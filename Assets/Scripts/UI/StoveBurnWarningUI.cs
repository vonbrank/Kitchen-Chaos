using System;
using Counters;
using UnityEngine;

namespace UI
{
    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private void OnEnable()
        {
            stoveCounter.OnStateChanged += HandleStateChanged;
            stoveCounter.OnProgressChanged += HandleProgressChanged;
        }

        private void OnDisable()
        {
            stoveCounter.OnStateChanged -= HandleStateChanged;
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

        private void HandleStateChanged(object sender, StoveCounter.StateChangedEventArgs e)
        {
            if (e.state == StoveCounter.State.Burned)
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