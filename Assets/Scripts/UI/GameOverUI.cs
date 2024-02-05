using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeDeliveredText;
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

        private void HandleStateChanged(object sender, KitchenGameManager.StateChangedEventArgs e)
        {
            if (e.state == KitchenGameManager.State.GameOver)
            {
                recipeDeliveredText.text = $"{DeliveryManager.Instance.SuccessfulRecipesAmount}";
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