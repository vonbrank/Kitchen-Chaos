using System;
using UnityEngine;

namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter baseCounter;
        [SerializeField] private GameObject[] visualGameObjects;

        private void OnEnable()
        {
            Player.Player.OnLocalInstanceChanged += HandlePlayerLocalInstanceChanged;
        }

        private void OnDisable()
        {
            Player.Player.OnLocalInstanceChanged -= HandlePlayerLocalInstanceChanged;
        }

        private void HandleSelectedCounterChange(object sender, Player.Player.SelectedCounterChangedEventArgs e)
        {
            if (e.BaseCounter == baseCounter)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void HandlePlayerLocalInstanceChanged(object sender, Player.Player.LocalInstanceChangedEventArgs e)
        {
            if (e.PreviousPlayer is not null)
            {
                e.PreviousPlayer.OnSelectedCounterChanged -= HandleSelectedCounterChange;
            }

            if (e.CurrentPlayer is not null)
            {
                e.CurrentPlayer.OnSelectedCounterChanged += HandleSelectedCounterChange;
            }
        }

        private void Show()
        {
            foreach (var gameObject in visualGameObjects)
            {
                gameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            foreach (var gameObject in visualGameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }
}