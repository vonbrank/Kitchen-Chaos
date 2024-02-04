using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            playerInputActions.Player.Interact.performed += InteractPerformed;
            playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
        }

        private void OnDisable()
        {
            playerInputActions.Player.Interact.performed -= InteractPerformed;
            playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
        }

        private void InteractPerformed(InputAction.CallbackContext context)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternatePerformed(InputAction.CallbackContext context)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
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