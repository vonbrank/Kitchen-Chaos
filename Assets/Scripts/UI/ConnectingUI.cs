using System;
using Managers;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class ConnectingUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI connectingMessageText;
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameMultiplayerManager.Instance.OnTryingToJoinGame += HandleTryingToJoinGame;
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame += HandleFailedToJoinGame;

            KitchenGameLobbyManager.Instance.OnCreateLobbyStarted += HandleStartLobby;
            KitchenGameLobbyManager.Instance.OnCreateLobbyFailed += HandleFailedToStartLobby;
            KitchenGameLobbyManager.Instance.OnJoinLobbyStarted += HandleJoinLobby;
            KitchenGameLobbyManager.Instance.OnJoinLobbyFailed += HandleFailedToJoinGame;
            KitchenGameLobbyManager.Instance.OnQuickJoinLobbyFailed += HandleFailedToJoinGame;
        }


        private void OnDisable()
        {
            KitchenGameMultiplayerManager.Instance.OnTryingToJoinGame -= HandleTryingToJoinGame;
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame -= HandleFailedToJoinGame;

            KitchenGameLobbyManager.Instance.OnCreateLobbyStarted -= HandleStartLobby;
            KitchenGameLobbyManager.Instance.OnCreateLobbyFailed -= HandleFailedToStartLobby;
            KitchenGameLobbyManager.Instance.OnJoinLobbyStarted -= HandleJoinLobby;
            KitchenGameLobbyManager.Instance.OnJoinLobbyFailed -= HandleFailedToJoinGame;
            KitchenGameLobbyManager.Instance.OnQuickJoinLobbyFailed -= HandleFailedToJoinGame;
        }

        private void Start()
        {
            Hide();
        }


        private void HandleTryingToJoinGame(object sender, EventArgs e)
        {
            Show();
        }

        private void HandleStartLobby(object sender, EventArgs e)
        {
            ShowMessage("Creating Lobby...");
        }

        private void HandleJoinLobby(object sender, EventArgs e)
        {
            ShowMessage("Joining Lobby...");
        }


        private void HandleFailedToStartLobby(object sender, EventArgs e)
        {
            Hide();
        }


        private void HandleFailedToJoinGame(object sender, EventArgs e)
        {
            Hide();
        }

        private void ShowMessage(string message)
        {
            connectingMessageText.text = message;
            Show();
        }

        private void Hide()
        {
            visualGameObjects.HideVisual();
        }

        private void Show()
        {
            visualGameObjects.ShowVisual();
        }
    }
}