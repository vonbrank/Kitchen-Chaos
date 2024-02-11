using System;
using Managers;
using UnityEngine;
using Utils;

namespace UI
{
    public class WaitingForOtherPlayersUI : MonoBehaviour
    {
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameManager.Instance.OnLocalPlayerReadyChanged += HandleLocalPlayerReadyChanged;
            KitchenGameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            KitchenGameManager.Instance.OnLocalPlayerReadyChanged -= HandleLocalPlayerReadyChanged;
            KitchenGameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        private void Start()
        {
            Hide();
        }

        private void HandleLocalPlayerReadyChanged(object sender, EventArgs e)
        {
            if (KitchenGameManager.Instance.IsLocalPlayerReady)
            {
                Show();
            }
        }

        private void HandleStateChanged(object sender, KitchenGameManager.StateChangedEventArgs e)
        {
            if (e.state == KitchenGameManager.State.CountDownToStart)
            {
                Hide();
            }
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