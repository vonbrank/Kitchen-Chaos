using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        private const string CUT = "Cut";

        [SerializeField] private CuttingCounter cuttingCounter;

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            cuttingCounter.OnCut += HandleCut;
        }

        private void OnDisable()
        {
            cuttingCounter.OnCut -= HandleCut;
        }

        private void HandleCut(object sender, EventArgs eventArgs)
        {
            animator.SetTrigger(CUT);
        }
    }
}