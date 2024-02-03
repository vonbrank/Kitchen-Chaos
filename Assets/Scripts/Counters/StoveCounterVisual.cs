using System;
using UnityEngine;

namespace Counters
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] private GameObject stoveGameObject;
        [SerializeField] private GameObject particlesGameObject;
        [SerializeField] private StoveCounter stoveCounter;

        private void OnEnable()
        {
            stoveCounter.OnStateChanged += HandleStateChange;
        }

        private void OnDisable()
        {
            stoveCounter.OnStateChanged -= HandleStateChange;
        }

        private void HandleStateChange(object sender, StoveCounter.StateChangedEventArgs eventArgs)
        {
            bool showVisual = eventArgs.state == StoveCounter.State.Frying ||
                              eventArgs.state == StoveCounter.State.Fried;
            stoveGameObject.SetActive(showVisual);
            particlesGameObject.SetActive(showVisual);
        }
    }
}