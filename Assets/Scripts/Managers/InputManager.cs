using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        
        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogError("There is more than one instance of InputManager.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            playerInputActions.Player.Interact.performed += InteractPerformed;
            playerInputActions.Player.InteractAlternate.performed += InteractAlternatePerformed;
            playerInputActions.Player.Pause.performed += PausePerformed;
        }

        private void OnDisable()
        {
            playerInputActions.Player.Interact.performed -= InteractPerformed;
            playerInputActions.Player.InteractAlternate.performed -= InteractAlternatePerformed;
            playerInputActions.Player.Pause.performed -= PausePerformed;
        }

        private void InteractPerformed(InputAction.CallbackContext context)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }

        private void InteractAlternatePerformed(InputAction.CallbackContext context)
        {
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
        }

        private void PausePerformed(InputAction.CallbackContext context)
        {
            OnPauseAction?.Invoke(this, EventArgs.Empty);
        }

        private void Start()
        {
            playerInputActions.Player.Enable();
        }

        private void OnDestroy()
        {
            playerInputActions.Dispose();

            Instance = null;
        }

        public Vector2 GetMovementVectorNormalized()
        {
            var inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector.Normalize();

            return inputVector;
        }
    }
}