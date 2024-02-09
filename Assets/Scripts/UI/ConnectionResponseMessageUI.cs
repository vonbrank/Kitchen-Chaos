using System;
using Managers;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class ConnectionResponseMessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame += HandleFailedToJoinGame;
            closeButton.onClick.AddListener(HandleCloseButtonClick);
        }

        private void OnDisable()
        {
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame -= HandleFailedToJoinGame;
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
            messageText.text = NetworkManager.Singleton.DisconnectReason;
            if (messageText.text == "")
            {
                messageText.text = "Failed to connect";
            }

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