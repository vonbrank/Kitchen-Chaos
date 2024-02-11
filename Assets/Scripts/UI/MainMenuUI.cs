using System;
using Managers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button singlePlayerButton;
        [SerializeField] private Button multiplayerButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            CleanUp();
        }

        private void OnEnable()
        {
            singlePlayerButton.onClick.AddListener(HandleSinglePlayer);
            multiplayerButton.onClick.AddListener(HandleMultiplayer);
            quitButton.onClick.AddListener(HandleQuit);
        }

        private void OnDisable()
        {
            singlePlayerButton.onClick.RemoveListener(HandleSinglePlayer);
            multiplayerButton.onClick.RemoveListener(HandleMultiplayer);
            quitButton.onClick.RemoveListener(HandleQuit);
        }

        private void Start()
        {
            singlePlayerButton.Select();
        }

        private void HandleSinglePlayer()
        {
            KitchenGameMultiplayerManager.PlayMultiplayer = false;
            SceneLoader.Load(SceneLoader.Scene.LobbyScene);
        }

        private void HandleMultiplayer()
        {
            KitchenGameMultiplayerManager.PlayMultiplayer = true;
            SceneLoader.Load(SceneLoader.Scene.LobbyScene);
        }

        private void HandleQuit()
        {
            Application.Quit();
        }

        private void CleanUp()
        {
            if (NetworkManager.Singleton)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }

            if (KitchenGameMultiplayerManager.Instance)
            {
                Destroy(KitchenGameMultiplayerManager.Instance.gameObject);
            }

            if (KitchenGameLobbyManager.Instance)
            {
                Destroy(KitchenGameLobbyManager.Instance.gameObject);
            }

            Time.timeScale = 1;
        }
    }
}