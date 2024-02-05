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
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameManager.Instance.OnGamePaused += HandleGamePaused;
            KitchenGameManager.Instance.OnGameResume += HandleGameResume;

            resumeButton.onClick.AddListener(HandleResumeButtonClick);
            mainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
        }

        private void OnDisable()
        {
            KitchenGameManager.Instance.OnGamePaused -= HandleGamePaused;
            KitchenGameManager.Instance.OnGameResume -= HandleGameResume;

            resumeButton.onClick.RemoveListener(HandleResumeButtonClick);
            mainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
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
        }

        private void Hide()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(false);
            }
        }
    }
}