using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour, IKitchenObjectParent
    {
        public static Player Instance { get; private set; }

        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotateSpeed = 10f;

        [SerializeField] private InputManager inputManager;
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
            if (Instance is not null)
            {
                Debug.LogError("There is more than one instance of Player.");
            }

            Instance = this;
        }

        private void OnEnable()
        {
            inputManager.OnInteractAction += OnInteractAction;
        }

        private void OnDisable()
        {
            inputManager.OnInteractAction -= OnInteractAction;
        }


        private void Update()
        {
            HandleMovement();
            HandleInteractions();
        }

        public bool IsWalking { get; private set; }

        private void HandleInteractions()
        {
            var inputVector = inputManager.GetMovementVectorNormalized();

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

        private void OnInteractAction(object sender, System.EventArgs e)
        {
            if (selectedCounter is not null)
            {
                selectedCounter.Interact(this);
            }
        }

        private void HandleMovement()
        {
            var inputVector = inputManager.GetMovementVectorNormalized();

            var moveDir = new Vector3(inputVector.x, 0, inputVector.y);

            float moveDistance = moveSpeed * Time.deltaTime;
            float playerRadius = 0.7f;
            float playerHeight = 2f;

            bool canMove = !Physics.CapsuleCast(
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
                    else
                    {
                    }
                }
            }

            if (canMove)
            {
                transform.position += moveDir * (moveSpeed * Time.deltaTime);
            }

            IsWalking = moveDir != Vector3.zero;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
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