using System;
using System.Collections.Generic;
using Managers;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button createLobbyButton;
        [SerializeField] private Button quickJoinButton;
        [SerializeField] private Button joinCodeButton;
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private CreateLobbyUI createLobbyUI;
        [SerializeField] private Transform[] visualGameObjects;
        [SerializeField] private Transform lobbyListTransform;
        [SerializeField] private LobbyNameButtonUI lobbyNameButtonUITemplate;

        private void OnEnable()
        {
            mainMenuButton.onClick.AddListener(HandleMainMenuButtonClick);
            createLobbyButton.onClick.AddListener(HandleCreateLobbyButtonClick);
            quickJoinButton.onClick.AddListener(HandleQuickJoinButtonClick);
            joinCodeButton.onClick.AddListener(HandleJoinCodeButtonClick);
            playerNameInputField.onValueChanged.AddListener(HandlePlayerNameInputValueChanged);
            KitchenGameLobbyManager.Instance.OnLobbyListChanged += HandleLobbyListChanged;
        }

        private void OnDisable()
        {
            mainMenuButton.onClick.RemoveListener(HandleMainMenuButtonClick);
            createLobbyButton.onClick.RemoveListener(HandleCreateLobbyButtonClick);
            quickJoinButton.onClick.RemoveListener(HandleQuickJoinButtonClick);
            joinCodeButton.onClick.RemoveListener(HandleJoinCodeButtonClick);
            playerNameInputField.onValueChanged.RemoveListener(HandlePlayerNameInputValueChanged);
            KitchenGameLobbyManager.Instance.OnLobbyListChanged -= HandleLobbyListChanged;
        }

        private void Start()
        {
            playerNameInputField.text = KitchenGameMultiplayerManager.Instance.PlayerName;
        }

        private void HandleMainMenuButtonClick()
        {
            KitchenGameLobbyManager.Instance.LeaveLobby();
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        }

        private void HandleCreateLobbyButtonClick()
        {
            createLobbyUI.OnClose += HandleCreateLobbyUIClose;
            Hide();
            createLobbyUI.Show();
        }

        private void HandleQuickJoinButtonClick()
        {
            KitchenGameLobbyManager.Instance.QuickJoin();
        }

        private void HandleJoinCodeButtonClick()
        {
            KitchenGameLobbyManager.Instance.JoinWithCode(joinCodeInputField.text);
        }

        private void HandleCreateLobbyUIClose(object sender, EventArgs e)
        {
            createLobbyUI.OnClose -= HandleCreateLobbyUIClose;
            Show();
        }

        private void HandlePlayerNameInputValueChanged(string newValue)
        {
            KitchenGameMultiplayerManager.Instance.SetPlayerName(newValue);
        }

        private void Hide()
        {
            visualGameObjects.HideVisual();
        }

        private void Show()
        {
            visualGameObjects.ShowVisual();
        }

        private void HandleLobbyListChanged(object sender, KitchenGameLobbyManager.OnLobbyListChangedEventArgs e)
        {
            UpdateLobbyList(e.Lobbies);
        }

        private void UpdateLobbyList(List<Lobby> lobbies)
        {
            foreach (Transform childTransform in lobbyListTransform)
            {
                if (childTransform == lobbyNameButtonUITemplate.transform)
                {
                    continue;
                }

                Destroy(childTransform.gameObject);
            }

            foreach (var lobby in lobbies)
            {
                LobbyNameButtonUI lobbyButtonUI = Instantiate(lobbyNameButtonUITemplate, lobbyListTransform);
                lobbyButtonUI.SetLobby(lobby);
                lobbyButtonUI.gameObject.SetActive(true);
            }
        }
    }
}