using System;
using System.Collections.Generic;
using Counters;
using KitchenObjects;
using Managers;
using Mono.CSharp;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        private static Player localInstance;

        public static Player LocalInstance
        {
            get => localInstance;
            set
            {
                var previousPlayer = localInstance;
                localInstance = value;
                var currentPlayer = localInstance;

                Debug.Log($"OnLocalInstanceChanged: {currentPlayer}");

                OnLocalInstanceChanged?.Invoke(null, new LocalInstanceChangedEventArgs
                {
                    PreviousPlayer = previousPlayer,
                    CurrentPlayer = currentPlayer,
                });
            }
        }

        public static event EventHandler<LocalInstanceChangedEventArgs> OnLocalInstanceChanged;

        public class LocalInstanceChangedEventArgs : EventArgs
        {
            public Player PreviousPlayer;
            public Player CurrentPlayer;
        }

        public static event EventHandler OnAnyPlayerPickupSomething;

        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotateSpeed = 10f;

        // [SerializeField] private InputManager inputManager;
        [SerializeField] private LayerMask countersLayerMask;
        [SerializeField] private LayerMask collisionsLayerMask;
        [SerializeField] private List<Vector3> spawnPositionList;
        [SerializeField] private PlayerVisual playerVisual;

        private Vector3 lastInteractDir;
        private BaseCounter selectedCounter;

        public event EventHandler<SelectedCounterChangedEventArgs> OnSelectedCounterChanged;

        public class SelectedCounterChangedEventArgs : EventArgs
        {
            public BaseCounter BaseCounter;
        }

        private void Awake()
        {
            // Instance = this;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                LocalInstance = this;
            }

            transform.position =
                spawnPositionList[KitchenGameMultiplayerManager.Instance.GetPlayerDataIndexByClientId(OwnerClientId)];

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsOwner)
            {
                LocalInstance = null;
            }

            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
            }
        }

        private void OnEnable()
        {
            InputManager.Instance.OnInteractAction += HandleInteractAction;
            InputManager.Instance.OnInteractAlternateAction += HandleInteractAlternateAction;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnInteractAction -= HandleInteractAction;
            InputManager.Instance.OnInteractAlternateAction -= HandleInteractAlternateAction;
        }


        private void Start()
        {
            PlayerData playerData = KitchenGameMultiplayerManager.Instance.GetPlayerDataByClientId(OwnerClientId);
            Color playerColor = KitchenGameMultiplayerManager.Instance.GetPlayerColor(playerData.ColorIndex);
            playerVisual.SetPlayerColor(playerColor);
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            HandleMovement();
            HandleInteractions();
        }

        public bool IsWalking { get; private set; }

        private void HandleInteractions()
        {
            var inputVector = InputManager.Instance.GetMovementVectorNormalized();

            var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            if (moveDir != Vector3.zero)
            {
                lastInteractDir = moveDir;
            }

            float interactDistance = 2f;
            if (Physics.Raycast(transform.position, lastInteractDir,
                    out RaycastHit raycastHit, interactDistance,
                    countersLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
                {
                    if (baseCounter != selectedCounter)
                    {
                        SetSelectedCounter(baseCounter);
                    }
                }
                else
                {
                    SetSelectedCounter(null);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }

            if (selectedCounter is not null)
            {
                // Debug.Log($"selected counter = {selectedCounter.transform}");
            }
        }


        private void HandleInteractAction(object sender, EventArgs e)
        {
            if (!KitchenGameManager.Instance.IsGamePlaying)
            {
                return;
            }

            if (selectedCounter is not null)
            {
                selectedCounter.Interact(this);
            }
        }

        private void HandleInteractAlternateAction(object sender, EventArgs e)
        {
            if (!KitchenGameManager.Instance.IsGamePlaying)
            {
                return;
            }

            if (selectedCounter is not null)
            {
                selectedCounter.InteractAlternate(this);
            }
        }

        private void HandleMovement()
        {
            var inputVector = InputManager.Instance.GetMovementVectorNormalized();

            var moveDir = new Vector3(inputVector.x, 0, inputVector.y);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);

            float moveDistance = moveSpeed * Time.deltaTime;
            float playerRadius = 0.7f;
            float playerHeight = 2f;

            bool canMove = (moveDir.magnitude > 0.5f) && !Physics.BoxCast(
                transform.position,
                Vector3.one * playerRadius,
                moveDir,
                Quaternion.identity,
                moveDistance,
                collisionsLayerMask);
            if (!canMove)
            {
                var moveDirX = new Vector3(inputVector.x, 0, 0).normalized;
                canMove = !Physics.BoxCast(
                    transform.position,
                    Vector3.one * playerRadius,
                    moveDirX,
                    Quaternion.identity,
                    moveDistance,
                    collisionsLayerMask);
                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    var moveDirZ = new Vector3(0, 0, inputVector.y).normalized;
                    canMove = !Physics.BoxCast(
                        transform.position,
                        Vector3.one * playerRadius,
                        moveDirZ,
                        Quaternion.identity,
                        moveDistance,
                        collisionsLayerMask);
                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                }
            }

            if (canMove)
            {
                transform.position += moveDir * (moveSpeed * Time.deltaTime);
            }

            IsWalking = moveDir != Vector3.zero;
        }

        private void SetSelectedCounter(BaseCounter baseCounter)
        {
            selectedCounter = baseCounter;
            OnSelectedCounterChanged?.Invoke(this, new SelectedCounterChangedEventArgs
            {
                BaseCounter = selectedCounter
            });
        }

        [SerializeField] private Transform kitchenObjectHoldPoint;
        private KitchenObject kitchenObject;
        public KitchenObject KitchenObject => kitchenObject;

        public Transform KitchenObjectFollowTransform => kitchenObjectHoldPoint;

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;

            if (this.kitchenObject)
            {
                OnAnyPlayerPickupSomething?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return kitchenObject is not null;
        }

        private void HandleClientDisconnect(ulong clientId)
        {
            if (clientId == OwnerClientId)
            {
                if (HasKitchenObject())
                {
                    KitchenObject.DestroyKitchenObject(KitchenObject);
                }
            }
        }
    }
}