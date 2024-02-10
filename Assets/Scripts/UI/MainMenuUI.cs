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
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            CleanUp();
        }

        private void OnEnable()
        {
            playButton.onClick.AddListener(HandlePlay);
            quitButton.onClick.AddListener(HandleQuit);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(HandlePlay);
            quitButton.onClick.RemoveListener(HandleQuit);
        }

        private void Start()
        {
            playButton.Select();
        }

        private void HandlePlay()
        {
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
        }
    }
}