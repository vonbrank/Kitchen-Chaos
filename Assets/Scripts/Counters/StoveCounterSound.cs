using System;
using UnityEngine;

namespace Counters
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            stoveCounter.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            stoveCounter.OnStateChanged -= HandleStateChanged;
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
    }
}