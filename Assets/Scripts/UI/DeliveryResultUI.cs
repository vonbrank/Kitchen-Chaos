using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeliveryResultUI : MonoBehaviour
    {
        private const string POPUP = "PopUp";

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Color successColor;
        [SerializeField] private Color failedColor;
        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failedSprite;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }


        private void OnEnable()
        {
            DeliveryManager.Instance.OnRecipeSuccess += HandleRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += HandleRecipeFailed;
        }

        private void OnDisable()
        {
            DeliveryManager.Instance.OnRecipeSuccess -= HandleRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed -= HandleRecipeFailed;
        }

        private void Start()
        {
            Hide();
        }

        private void HandleRecipeSuccess(object sender, EventArgs e)
        {
            animator.SetTrigger(POPUP);
            backgroundImage.color = successColor;
            iconImage.sprite = successSprite;
            messageText.text = "DELIVERY\nSUCCESS";
            Show();
        }


        private void HandleRecipeFailed(object sender, EventArgs e)
        {
            animator.SetTrigger(POPUP);
            backgroundImage.color = failedColor;
            iconImage.sprite = failedSprite;
            messageText.text = "DELIVERY\nFAILED";
            Show();
        }

        [SerializeField] private Transform[] visualGameObjects;


        public void Show()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(true);
            }
        }

        public void Hide()
        {
            foreach (Transform visualTransform in visualGameObjects)
            {
                visualTransform.gameObject.SetActive(false);
            }
        }
    }
}