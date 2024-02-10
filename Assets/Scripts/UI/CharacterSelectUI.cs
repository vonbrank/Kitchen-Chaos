using System;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button readyButton;

        private void OnEnable()
        {
            mainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
            readyButton.onClick.AddListener(HandleReadyButtonClick);
        }

        private void OnDisable()
        {
            mainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
            readyButton.onClick.RemoveListener(HandleReadyButtonClick);
        }

        private void HandleMainMenuButtonClick()
        {
            NetworkManager.Singleton.Shutdown();
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        }

        private void HandleReadyButtonClick()
        {
            PlayerSelectReadyManager.Instance.PlayerSelectReady();
        }
    }
}