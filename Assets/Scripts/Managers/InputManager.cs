using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private const string PLAYER_PREFS_BIDINGS = "InputBindings";

        public static InputManager Instance { get; private set; }

        public event EventHandler OnInteractAction;
        public event EventHandler OnInteractAlternateAction;
        public event EventHandler OnPauseAction;

        public event EventHandler OnKeyRebind;

        public enum Binding
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            Interact,
            InteractAlt,
            Pause,
            GamepadInteract,
            GamepadInteractAlt,
            GamepadPause,
        }

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

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BIDINGS))
            {
                playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BIDINGS));
            }
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
        }

        public Vector2 GetMovementVectorNormalized()
        {
            var inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector.Normalize();

            return inputVector;
        }

        public string GetBindingText(Binding binding)
        {
            switch (binding)
            {
                default:

                case Binding.MoveUp:
                    return playerInputActions.Player.Move.bindings[1].ToDisplayString();
                case Binding.MoveDown:
                    return playerInputActions.Player.Move.bindings[2].ToDisplayString();
                case Binding.MoveLeft:
                    return playerInputActions.Player.Move.bindings[3].ToDisplayString();
                case Binding.MoveRight:
                    return playerInputActions.Player.Move.bindings[4].ToDisplayString();
                case Binding.Interact:
                    return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
                case Binding.InteractAlt:
                    return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
                case Binding.Pause:
                    return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
                case Binding.GamepadInteract:
                    return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
                case Binding.GamepadInteractAlt:
                    return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
                case Binding.GamepadPause:
                    return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
            }
        }

        public void RebindBinding(Binding binding, Action onRebindComplete)
        {
            (InputAction, int) inputActionAndBindingIndex = binding switch
            {
                Binding.MoveUp => (playerInputActions.Player.Move, 1),
                Binding.MoveDown => (playerInputActions.Player.Move, 2),
                Binding.MoveLeft => (playerInputActions.Player.Move, 3),
                Binding.MoveRight => (playerInputActions.Player.Move, 4),
                Binding.Interact => (playerInputActions.Player.Interact, 0),
                Binding.InteractAlt => (playerInputActions.Player.InteractAlternate, 0),
                Binding.Pause => (playerInputActions.Player.Pause, 0),
                Binding.GamepadInteract => (playerInputActions.Player.Interact, 1),
                Binding.GamepadInteractAlt => (playerInputActions.Player.InteractAlternate, 1),
                Binding.GamepadPause => (playerInputActions.Player.Pause, 1),
                _ => (null, 0)
            };

            if (inputActionAndBindingIndex.Item1 is null)
            {
                return;
            }

            playerInputActions.Player.Disable();

            inputActionAndBindingIndex.Item1.PerformInteractiveRebinding(inputActionAndBindingIndex.Item2)
                .OnComplete(callback =>
                {
                    playerInputActions.Player.Enable();
                    callback.Dispose();

                    onRebindComplete?.Invoke();
                    OnKeyRebind?.Invoke(this, EventArgs.Empty);

                    PlayerPrefs.SetString(PLAYER_PREFS_BIDINGS, playerInputActions.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();
                }).Start();
        }
    }
}