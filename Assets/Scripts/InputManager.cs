using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            playerInputActions = new PlayerInputActions();
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