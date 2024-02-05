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
            Player.Player.Instance.OnSelectedCounterChanged += HandleSelectedCounterChange;
        }

        private void OnDisable()
        {
            Player.Player.Instance.OnSelectedCounterChanged -= HandleSelectedCounterChange;
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