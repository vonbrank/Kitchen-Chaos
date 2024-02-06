using System;
using Counters;
using KitchenObjects;
using Managers;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        // public static Player Instance { get; private set; }

        public event EventHandler OnPlayerPickupSomething;

        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotateSpeed = 10f;

        // [SerializeField] private InputManager inputManager;
        [SerializeField] private LayerMask countersLayerMask;

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


        private void Update()
        {
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

            bool canMove = (moveDir.magnitude > 0.5f) && !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDir,
                moveDistance);
            if (!canMove)
            {
                var moveDirX = new Vector3(inputVector.x, 0, 0).normalized;
                canMove = !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirX,
                    moveDistance);
                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    var moveDirZ = new Vector3(0, 0, inputVector.y).normalized;
                    canMove = !Physics.CapsuleCast(
                        transform.position,
                        transform.position + Vector3.up * playerHeight,
                        playerRadius,
                        moveDirZ,
                        moveDistance);
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
                OnPlayerPickupSomething?.Invoke(this, EventArgs.Empty);
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
    }
}