using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountDownUI : MonoBehaviour
    {
        private const string NUMBER_POPUP = "NumberPopUp";

        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private Transform[] visualGameObjects;

        private Animator animator;
        private int previousCountDownNumber;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

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
                int currentCountDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.RestCountDownTime);
                if (currentCountDownNumber != previousCountDownNumber)
                {
                    animator.SetTrigger(NUMBER_POPUP);
                    SoundManager.Instance.PlayCountDownSound();
                }

                countDownText.text = $"{currentCountDownNumber}";
                previousCountDownNumber = currentCountDownNumber;
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