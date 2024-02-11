using System;
using Managers;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class CharacterSelectUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button readyButton;
        [SerializeField] private TextMeshProUGUI lobbyNameText;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

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


        private void Start()
        {
            Lobby lobby = KitchenGameLobbyManager.Instance.JoinedLobby;

            lobbyNameText.text = $"Lobby Name: {lobby.Name}";
            lobbyCodeText.text = $"Lobby Code: {lobby.LobbyCode}";
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