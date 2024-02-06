using System;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : NetworkBehaviour
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
            if (!IsOwner)
            {
                return;
            }

            animator.SetBool(IS_WALKING, player.IsWalking);
        }
    }
}