using System;
using System.Collections;
using DefaultNamespace;
using Managers;
using UnityEngine;

namespace Counters
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private AudioSource audioSource;
        private bool playWarningSound;
        private Coroutine playWarningSoundCoroutine;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

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
            playWarningSoundCoroutine = StartCoroutine(HandlePlayWarningSound());
        }

        private void OnDestroy()
        {
            if (playWarningSoundCoroutine is not null)
            {
                StopCoroutine(playWarningSoundCoroutine);
            }
        }

        private void HandleStateChanged(object sender, StoveCounter.StateChangedEventArgs e)
        {
            bool shouldPlaySound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
            if (shouldPlaySound)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Pause();
            }
        }

        private void HandleProgressChanged(object sender, IHasProgress.ProgressChangedEventArgs e)
        {
            float burnShowProgressThreshold = 0.5f;
            playWarningSound = stoveCounter.IsFried && e.progressNormalized >= burnShowProgressThreshold;
        }

        private IEnumerator HandlePlayWarningSound()
        {
            while (true)
            {
                float timeElapsed = 0;
                float maxPlayWarningSoundTime = 0.2f;
                while (timeElapsed < maxPlayWarningSoundTime)
                {
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                if (playWarningSound)
                {
                    SoundManager.Instance.PlayWarningSound(transform.position);
                }
            }
        }
    }
}