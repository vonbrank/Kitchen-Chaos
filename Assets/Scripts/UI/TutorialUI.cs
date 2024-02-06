using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI interactAltText;
        [SerializeField] private TextMeshProUGUI pauseText;
        [SerializeField] private TextMeshProUGUI gamepadInteractText;
        [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
        [SerializeField] private TextMeshProUGUI gamepadPauseText;

        private void OnEnable()
        {
            InputManager.Instance.OnKeyRebind += HandleKeyRebind;
            KitchenGameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnKeyRebind -= HandleKeyRebind;
            KitchenGameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        private void HandleKeyRebind(object sender, EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            moveUpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveUp);
            moveDownText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveDown);
            moveLeftText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveLeft);
            moveRightText.text = InputManager.Instance.GetBindingText(InputManager.Binding.MoveRight);
            interactText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Interact);
            interactAltText.text = InputManager.Instance.GetBindingText(InputManager.Binding.InteractAlt);
            pauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Pause);
            gamepadInteractText.text = InputManager.Instance.GetBindingText(InputManager.Binding.GamepadInteract);
            gamepadInteractAltText.text = InputManager.Instance.GetBindingText(InputManager.Binding.GamepadInteractAlt);
            gamepadPauseText.text = InputManager.Instance.GetBindingText(InputManager.Binding.GamepadPause);
        }


        [SerializeField] private Transform[] visualGameObjects;


        public void Show()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(false);
            }
        }

        private void HandleStateChanged(object sender, KitchenGameManager.StateChangedEventArgs e)
        {
            if (e.state == KitchenGameManager.State.WaitingToStart)
            {
                UpdateVisual();
                Show();
            }
            else if (e.state == KitchenGameManager.State.CountDownToStart)
            {
                Hide();
            }
        }
    }
}