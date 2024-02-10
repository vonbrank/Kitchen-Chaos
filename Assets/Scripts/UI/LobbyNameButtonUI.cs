using System;
using Managers;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LobbyNameButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;
        private Lobby lobby;
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(HandleButtonClick);
        }

        public void SetLobby(Lobby lobby)
        {
            this.lobby = lobby;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            lobbyNameText.text = lobby.Name;
        }

        private void HandleButtonClick()
        {
            KitchenGameLobbyManager.Instance.JoinWithId(lobby.Id);
        }
    }
}