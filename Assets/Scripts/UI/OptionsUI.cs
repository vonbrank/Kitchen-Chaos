using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] private Button soundEffectsButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button moveUpButton;
        [SerializeField] private Button moveDownButton;
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button moveRightButton;
        [SerializeField] private Button interactButton;
        [SerializeField] private Button interactAltButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button gamepadInteractButton;
        [SerializeField] private Button gamepadInteractAltButton;
        [SerializeField] private Button gamepadPauseButton;

        [SerializeField] private TextMeshProUGUI soundEffectsText;
        [SerializeField] private TextMeshProUGUI musicText;
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

        [SerializeField] private Transform pressToRebindKeyGameObject;

        public event Action OnClose;

        private void OnEnable()
        {
            soundEffectsButton.onClick.AddListener(HandleSoundEffectsButtonClick);
            musicButton.onClick.AddListener(HandleMusicButtonClick);
            closeButton.onClick.AddListener(HandleCloseButtonClick);

            moveUpButton.onClick.AddListener(HandleMoveUpButtonClick);
            moveDownButton.onClick.AddListener(HandleMoveDownButtonClick);
            moveLeftButton.onClick.AddListener(HandleMoveLeftButtonClick);
            moveRightButton.onClick.AddListener(HandleMoveRightButtonClick);
            interactButton.onClick.AddListener(HandleInteractButtonClick);
            interactAltButton.onClick.AddListener(HandleInteractAltButtonClick);
            pauseButton.onClick.AddListener(HandlePauseButtonClick);

            gamepadInteractButton.onClick.AddListener(HandleGamepadInteractButtonClick);
            gamepadInteractAltButton.onClick.AddListener(HandleGamepadInteractAltButtonClick);
            gamepadPauseButton.onClick.AddListener(HandleGamepadPauseButtonClick);
        }

        private void OnDisable()
        {
            soundEffectsButton.onClick.RemoveListener(HandleSoundEffectsButtonClick);
            musicButton.onClick.RemoveListener(HandleMusicButtonClick);
            closeButton.onClick.RemoveListener(HandleCloseButtonClick);

            moveUpButton.onClick.RemoveListener(HandleMoveUpButtonClick);
            moveDownButton.onClick.RemoveListener(HandleMoveDownButtonClick);
            moveLeftButton.onClick.RemoveListener(HandleMoveLeftButtonClick);
            moveRightButton.onClick.RemoveListener(HandleMoveRightButtonClick);
            interactButton.onClick.RemoveListener(HandleInteractButtonClick);
            interactAltButton.onClick.RemoveListener(HandleInteractAltButtonClick);
            pauseButton.onClick.RemoveListener(HandlePauseButtonClick);

            gamepadInteractButton.onClick.RemoveListener(HandleGamepadInteractButtonClick);
            gamepadInteractAltButton.onClick.RemoveListener(HandleGamepadInteractAltButtonClick);
            gamepadPauseButton.onClick.RemoveListener(HandleGamepadPauseButtonClick);
        }

        private void Start()
        {
            UpdateVisual();

            Hide();
        }

        private void HandleSoundEffectsButtonClick()
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        }

        private void HandleMusicButtonClick()
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        }

        private void HandleCloseButtonClick()
        {
            Hide();
            OnClose?.Invoke();
        }

        private void UpdateVisual()
        {
            soundEffectsText.text = $"Sound Effects: {Mathf.RoundToInt(SoundManager.Instance.VolumeMultiplier * 10)}";
            musicText.text = $"Music: {Mathf.RoundToInt(MusicManager.Instance.VolumeMultiplier * 10)}";

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

            soundEffectsButton.Select();
        }

        public void Hide()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(false);
            }

            HidePressToRebindKey();
        }

        private void ShowPressToRebindKey()
        {
            pressToRebindKeyGameObject.gameObject.SetActive(true);
        }

        private void HidePressToRebindKey()
        {
            pressToRebindKeyGameObject.gameObject.SetActive(false);
        }

        private void HandleRebindComplete()
        {
            UpdateVisual();
            HidePressToRebindKey();
        }

        private void HandleMoveUpButtonClick() => RebindBinding(InputManager.Binding.MoveUp);
        private void HandleMoveDownButtonClick() => RebindBinding(InputManager.Binding.MoveDown);
        private void HandleMoveLeftButtonClick() => RebindBinding(InputManager.Binding.MoveLeft);
        private void HandleMoveRightButtonClick() => RebindBinding(InputManager.Binding.MoveRight);
        private void HandleInteractButtonClick() => RebindBinding(InputManager.Binding.Interact);
        private void HandleInteractAltButtonClick() => RebindBinding(InputManager.Binding.InteractAlt);
        private void HandlePauseButtonClick() => RebindBinding(InputManager.Binding.Pause);
        private void HandleGamepadInteractButtonClick() => RebindBinding(InputManager.Binding.GamepadInteract);
        private void HandleGamepadInteractAltButtonClick() => RebindBinding(InputManager.Binding.GamepadInteractAlt);
        private void HandleGamepadPauseButtonClick() => RebindBinding(InputManager.Binding.GamepadPause);

        private void RebindBinding(InputManager.Binding binding)
        {
            ShowPressToRebindKey();
            InputManager.Instance.RebindBinding(binding, HandleRebindComplete);
        }
    }
}