using System;
using Counters;
using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private void OnEnable()
        {
            stoveCounter.OnProgressChanged += HandleProgressChanged;
        }

        private void OnDisable()
        {
            stoveCounter.OnProgressChanged -= HandleProgressChanged;
        }

        private void Start()
        {
            Hide();
        }

        private void HandleProgressChanged(object sender, IHasProgress.ProgressChangedEventArgs e)
        {
            float burnShowProgressThreshold = 0.5f;
            bool show = stoveCounter.IsFried && e.progressNormalized >= burnShowProgressThreshold;

            if (show)
            {
                Show();
            }
            else
            {
                Hide();
            }
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