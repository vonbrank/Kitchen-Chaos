using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TestingNetcodeUI : MonoBehaviour
    {
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            startHostButton.onClick.AddListener(HandleHost);
            startClientButton.onClick.AddListener(HandleClient);
        }

        private void OnDisable()
        {
            startHostButton.onClick.RemoveListener(HandleHost);
            startClientButton.onClick.RemoveListener(HandleClient);
        }

        private void HandleHost()
        {
            Debug.Log("Start Host...");
            NetworkManager.Singleton.StartHost();
            visualGameObjects.HideVisual();
        }

        private void HandleClient()
        {
            Debug.Log("Start Client...");
            NetworkManager.Singleton.StartClient();
            visualGameObjects.HideVisual();
        }
    }
}