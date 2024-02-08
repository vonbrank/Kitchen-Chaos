using System;
using Managers;
using UnityEngine;
using Utils;

namespace UI
{
    public class WaitingForAllPlayersResumeUI : MonoBehaviour
    {
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameManager.Instance.OnNetworkGamePaused += HandleNetworkPaused;
            KitchenGameManager.Instance.OnNetworkGameResume += HandleNetworkResumed;
        }

        private void OnDisable()
        {
            KitchenGameManager.Instance.OnNetworkGamePaused -= HandleNetworkPaused;
            KitchenGameManager.Instance.OnNetworkGameResume -= HandleNetworkResumed;
        }

        private void Start()
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

        private void HandleNetworkPaused(object sender, EventArgs e)
        {
            Show();
        }

        private void HandleNetworkResumed(object sender, EventArgs e)
        {
            Hide();
        }
    }
}