using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputManager : MonoBehaviour
    {
        public event EventHandler OnInteractAction;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            playerInputActions.Player.Interact.performed += InteractPerformed;
        }

        private void OnDisable()
        {
            playerInputActions.Player.Interact.performed -= InteractPerformed;
        }

        private void InteractPerformed(InputAction.CallbackContext context)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        private void Start()
        {
            playerInputActions.Player.Enable();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            var inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector.Normalize();

            return inputVector;
        }
    }
}