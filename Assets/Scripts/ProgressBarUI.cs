using System;
using Counters;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private CuttingCounter cuttingCounter;
        [SerializeField] private Image barImage;
        [SerializeField] private GameObject[] visualGameObjects;

        private void Start()
        {
            barImage.fillAmount = 0f;

            Hide();
        }

        private void OnEnable()
        {
            cuttingCounter.OnProgressChanged += HandleProgressChanged;
        }

        private void OnDisable()
        {
            cuttingCounter.OnProgressChanged -= HandleProgressChanged;
        }

        private void HandleProgressChanged(object sender, CuttingCounter.ProgressChangedEventArgs eventArgs)
        {
            barImage.fillAmount = eventArgs.progressNormalized;

            if (eventArgs.progressNormalized == 0f || eventArgs.progressNormalized == 1f)
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