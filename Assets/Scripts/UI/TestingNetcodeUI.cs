using System;
using Managers;
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
            KitchenGameMultiplayerManager.Instance.StartHost();
            visualGameObjects.HideVisual();
        }

        private void HandleClient()
        {
            KitchenGameMultiplayerManager.Instance.StartClient();
            visualGameObjects.HideVisual();
        }
    }
}