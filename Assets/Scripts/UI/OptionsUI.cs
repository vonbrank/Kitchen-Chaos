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
        [SerializeField] private TextMeshProUGUI soundEffectsText;
        [SerializeField] private TextMeshProUGUI musicText;

        private void OnEnable()
        {
            soundEffectsButton.onClick.AddListener(HandleSoundEffectsButtonClick);
            musicButton.onClick.AddListener(HandleMusicButtonClick);
            closeButton.onClick.AddListener(HandleCloseButtonClick);
        }

        private void OnDisable()
        {
            soundEffectsButton.onClick.RemoveListener(HandleSoundEffectsButtonClick);
            musicButton.onClick.RemoveListener(HandleMusicButtonClick);
            closeButton.onClick.RemoveListener(HandleCloseButtonClick);
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
        }

        private void UpdateVisual()
        {
            soundEffectsText.text = $"Sound Effects: {Mathf.RoundToInt(SoundManager.Instance.VolumeMultiplier * 10)}";
            musicText.text = $"Music: {Mathf.RoundToInt(MusicManager.Instance.VolumeMultiplier * 10)}";
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
    }
}