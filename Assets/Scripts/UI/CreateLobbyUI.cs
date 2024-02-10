using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class CreateLobbyUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button createPublicButton;
        [SerializeField] private Button createPrivateButton;
        [SerializeField] private TMP_InputField lobbyNameInputField;
        [SerializeField] private Transform[] visualGameObjects;

        public event EventHandler OnClose;


        private void OnEnable()
        {
            closeButton.onClick.AddListener(HandleClose);
            createPublicButton.onClick.AddListener(HandleCreatePublic);
            createPrivateButton.onClick.AddListener(HandleCreatePrivate);
        }

        private void OnDisable()
        {
            closeButton.onClick.RemoveListener(HandleClose);
            createPublicButton.onClick.RemoveListener(HandleCreatePublic);
            createPrivateButton.onClick.RemoveListener(HandleCreatePrivate);
        }

        private void Start()
        {
            Hide();
        }

        private void HandleClose()
        {
            Hide();
            OnClose?.Invoke(this, EventArgs.Empty);
        }

        private void HandleCreatePublic()
        {
            KitchenGameLobbyManager.Instance.CreateLobby(lobbyNameInputField.text, false);
        }

        private void HandleCreatePrivate()
        {
            KitchenGameLobbyManager.Instance.CreateLobby(lobbyNameInputField.text, true);
        }

        public void Hide()
        {
            visualGameObjects.HideVisual();
        }

        public void Show()
        {
            visualGameObjects.ShowVisual();
        }
    }
}