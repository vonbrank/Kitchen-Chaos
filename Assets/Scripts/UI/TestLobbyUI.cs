using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TestLobbyUI : MonoBehaviour
    {
        [SerializeField] private Button createGameButton;
        [SerializeField] private Button joinGameButton;

        private void OnEnable()
        {
            createGameButton.onClick.AddListener(HandleCreateGameButtonClick);
            joinGameButton.onClick.AddListener(HandleJoinGameButtonClick);
        }

        private void OnDisable()
        {
            createGameButton.onClick.RemoveListener(HandleCreateGameButtonClick);
            createGameButton.onClick.RemoveListener(HandleCreateGameButtonClick);
        }

        private void HandleCreateGameButtonClick()
        {
            KitchenGameMultiplayerManager.Instance.StartHost();
            SceneLoader.LoadNetwork(SceneLoader.Scene.CharacterSelectScene);
        }

        private void HandleJoinGameButtonClick()
        {
            KitchenGameMultiplayerManager.Instance.StartClient();
        }
    }
}