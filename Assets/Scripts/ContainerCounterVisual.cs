using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContainerCounterVisual : MonoBehaviour
    {
        private const string OPEN_CLOSE = "OpenClose";

        [SerializeField] private ContainerCounter containerCounter;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            containerCounter.OnPlayerGrabObject += HandlePlayerGrabObject;
        }

        private void OnDisable()
        {
            containerCounter.OnPlayerGrabObject -= HandlePlayerGrabObject;
        }

        private void HandlePlayerGrabObject(object sender, EventArgs e)
        {
            animator.SetTrigger(OPEN_CLOSE);
        }
    }
}