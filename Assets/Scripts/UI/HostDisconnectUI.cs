using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class HostDisconnectUI : MonoBehaviour
    {
        [SerializeField] private Transform[] visualGameObjects;
        [SerializeField] private Button playAgainButton;
        private NetworkManager instance;

        private void OnEnable()
        {
            instance = NetworkManager.Singleton;
            instance.OnClientDisconnectCallback += HandleClientDisconnect;
            playAgainButton.onClick.AddListener(HandlePlayAgainButtonClick);
        }

        private void OnDisable()
        {
            playAgainButton.onClick.RemoveListener(HandlePlayAgainButtonClick);
            instance.OnClientDisconnectCallback -= HandleClientDisconnect;
            instance = null;
        }

        private void Start()
        {
            Hide();
        }


        private void HandleClientDisconnect(ulong clientId)
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                Show();
            }
        }

        private void Hide()
        {
            visualGameObjects.HideVisual();
        }

        private void Show()
        {
            visualGameObjects.ShowVisual();
            playAgainButton.Select();
        }

        private void HandlePlayAgainButtonClick()
        {
            instance.Shutdown();
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        }
    }
}