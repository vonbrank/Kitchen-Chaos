using System;
using DefaultNamespace;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Transform[] visualGameObjects;
        [SerializeField] private OptionsUI optionsUI;

        private void OnEnable()
        {
            KitchenGameManager.Instance.OnGamePaused += HandleGamePaused;
            KitchenGameManager.Instance.OnGameResume += HandleGameResume;

            resumeButton.onClick.AddListener(HandleResumeButtonClick);
            mainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
            optionsButton.onClick.AddListener(HandleOptionsButtonClick);
        }

        private void OnDisable()
        {
            KitchenGameManager.Instance.OnGamePaused -= HandleGamePaused;
            KitchenGameManager.Instance.OnGameResume -= HandleGameResume;

            resumeButton.onClick.RemoveListener(HandleResumeButtonClick);
            mainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
            optionsButton.onClick.RemoveListener(HandleOptionsButtonClick);
        }

        private void Start()
        {
            Hide();
        }

        private void HandleGamePaused(object sender, EventArgs e)
        {
            Show();
        }

        private void HandleGameResume(object sender, EventArgs e)
        {
            Hide();
        }

        private void HandleOptionsButtonClick()
        {
            Hide();
            optionsUI.Show();
            optionsUI.OnClose += HandleOptionsUIClose;
        }

        private void HandleMainMenuButtonClick()
        {
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
            KitchenGameManager.Instance.TogglePauseGame();
        }

        private void HandleResumeButtonClick()
        {
            KitchenGameManager.Instance.TogglePauseGame();
        }

        private void Show()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(true);
            }

            resumeButton.Select();
        }

        private void Hide()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(false);
            }

            optionsUI.Hide();
        }

        private void HandleOptionsUIClose()
        {
            optionsUI.OnClose -= HandleOptionsUIClose;
            Show();
        }
    }
}