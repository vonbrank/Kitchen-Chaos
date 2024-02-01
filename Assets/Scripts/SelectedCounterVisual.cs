using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private ClearCounter clearCounter;
        [SerializeField] private GameObject visualGameObject;

        private void OnEnable()
        {
            Player.Instance.OnSelectedCounterChanged += HandleSelectedCounterChange;
        }

        private void OnDisable()
        {
            Player.Instance.OnSelectedCounterChanged -= HandleSelectedCounterChange;
        }

        private void HandleSelectedCounterChange(object sender, Player.SelectedCounterChangedEventArgs e)
        {
            if (e.SelectedCounter == clearCounter)
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
            visualGameObject.SetActive(true);
        }

        private void Hide()
        {
            visualGameObject.SetActive(false);
        }
    }
}