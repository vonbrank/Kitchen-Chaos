using System;
using Counters;
using UnityEngine;

namespace UI
{
    public class StoveBurnFlashingBarUI : MonoBehaviour
    {
        private const string IS_FLASHING = "IsFlashing";

        [SerializeField] private StoveCounter stoveCounter;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            stoveCounter.OnProgressChanged += HandleProgressChanged;
        }

        private void OnDisable()
        {
            stoveCounter.OnProgressChanged -= HandleProgressChanged;
        }

        private void HandleProgressChanged(object sender, IHasProgress.ProgressChangedEventArgs e)
        {
            float burnShowProgressThreshold = 0.5f;
            bool isFlashing = stoveCounter.IsFried && e.progressNormalized >= burnShowProgressThreshold;

            animator.SetBool(IS_FLASHING, isFlashing);
        }
    }
}