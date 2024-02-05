using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountDownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private Transform[] visualGameObjects;

        private void OnEnable()
        {
            KitchenGameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            KitchenGameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            if (KitchenGameManager.Instance.IsCountingDownToStartActive)
            {
                countDownText.text = $"{Mathf.Ceil(KitchenGameManager.Instance.ResetCountDownTime)}";
            }
        }

        private void HandleStateChanged(object sender, KitchenGameManager.StateChangedEventArgs e)
        {
            if (e.state == KitchenGameManager.State.CountDownToStart)
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
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(false);
            }
        }
    }
}