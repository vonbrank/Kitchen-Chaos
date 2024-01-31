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

            IsWalking = moveDir != Vector3.zero;

            transform.position += moveDir * (moveSpeed * Time.deltaTime);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }

        public bool IsWalking { get; private set; }
    }
}