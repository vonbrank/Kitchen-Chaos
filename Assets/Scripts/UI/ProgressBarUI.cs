using System;
using Counters;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private GameObject hasProgressGameObject;
        [SerializeField] private Image barImage;
        [SerializeField] private GameObject[] visualGameObjects;

        private IHasProgress hasProgress;

        private void Awake()
        {
            hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
            if (hasProgress is null)
            {
                Debug.LogError(
                    $"Game Object {hasProgressGameObject} does not have a component that implements IHasProgress.");
            }
        }

        private void Start()
        {
            barImage.fillAmount = 0f;

            Hide();
        }

        private void OnEnable()
        {
            hasProgress.OnProgressChanged += HandleProgressChanged;
        }

        private void OnDisable()
        {
            hasProgress.OnProgressChanged -= HandleProgressChanged;
        }

        private void HandleProgressChanged(object sender, IHasProgress.ProgressChangedEventArgs eventArgs)
        {
            barImage.fillAmount = eventArgs.progressNormalized;

            if (eventArgs.progressNormalized <= 0f || eventArgs.progressNormalized >= 1f)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            foreach (GameObject visualGameObject in visualGameObjects)
            {
                visualGameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            foreach (GameObject visualGameObject in visualGameObjects)
            {
                visualGameObject.SetActive(false);
            }
        }
    }
}