using System;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LobbyMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame += HandleFailedToJoinGame;
            KitchenGameLobbyManager.Instance.OnCreateLobbyFailed += HandleFailedToStartLobby;
            KitchenGameLobbyManager.Instance.OnJoinLobbyFailed += HandleFailedToJoinLobby;
            KitchenGameLobbyManager.Instance.OnQuickJoinLobbyFailed += HandleFailedToQuickJoinLobby;
            closeButton.onClick.AddListener(HandleCloseButtonClick);
        }

        private void OnDisable()
        {
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame -= HandleFailedToJoinGame;
            KitchenGameLobbyManager.Instance.OnCreateLobbyFailed -= HandleFailedToStartLobby;
            KitchenGameLobbyManager.Instance.OnJoinLobbyFailed -= HandleFailedToJoinLobby;
            KitchenGameLobbyManager.Instance.OnQuickJoinLobbyFailed -= HandleFailedToQuickJoinLobby;
            closeButton.onClick.RemoveListener(HandleCloseButtonClick);
        }

        private void Start()
        {
            Hide();
        }

        private void HandleCloseButtonClick()
        {
            Hide();
        }

        private void HandleFailedToJoinGame(object sender, EventArgs e)
        {
            if (NetworkManager.Singleton.DisconnectReason == "")
            {
                ShowMessage("Failed to connect");
            }
            else
            {
                ShowMessage(NetworkManager.Singleton.DisconnectReason);
            }
        }

        private void HandleFailedToStartLobby(object sender, EventArgs e)
        {
            ShowMessage("Failed to create Lobby!");
        }

        private void HandleFailedToQuickJoinLobby(object sender, EventArgs e)
        {
            ShowMessage("Could not find a lobby to Quick Join!");
        }

        private void HandleFailedToJoinLobby(object sender, EventArgs e)
        {
            ShowMessage("Failed to Join Lobby");
        }

        private void ShowMessage(string message)
        {
            messageText.text = message;
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