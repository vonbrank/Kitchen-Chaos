using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotateSpeed = 10f;

        [SerializeField] private InputManager inputManager;

        private void Update()
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

        public bool IsWalking { get; private set; }
    }
}