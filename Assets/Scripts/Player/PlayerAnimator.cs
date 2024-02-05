using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private const string IS_WALKING = "IsWalking";

        private Animator animator;

        [SerializeField] private Player player;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            animator.SetBool(IS_WALKING, player.IsWalking);
        }
    }
}