using System;
using Managers;
using UnityEngine;
using Utils;

namespace UI
{
    public class ConnectingUI : MonoBehaviour
    {
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameMultiplayerManager.Instance.OnTryingToJoinGame += HandleTryingToJoinGame;
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame += HandleFailedToJoinGame;
        }


        private void OnDisable()
        {
            KitchenGameMultiplayerManager.Instance.OnTryingToJoinGame -= HandleTryingToJoinGame;
            KitchenGameMultiplayerManager.Instance.OnFailedToJoinGame -= HandleFailedToJoinGame;
        }

        private void Start()
        {
            Hide();
        }


        private void HandleTryingToJoinGame(object sender, EventArgs e)
        {
            Show();
        }


        private void HandleFailedToJoinGame(object sender, EventArgs e)
        {
            Hide();
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