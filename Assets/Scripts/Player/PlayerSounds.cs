using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private float maxFootStepTime = 0.1f;
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void Start()
        {
            StartCoroutine(HandleFootStep());
        }

        private IEnumerator HandleFootStep()
        {
            while (true)
            {
                float timeElapsed = 0f;
                while (timeElapsed < maxFootStepTime)
                {
                    yield return null;
                    timeElapsed += Time.deltaTime;
                }

                if (player.IsWalking)
                {
                    SoundManager.Instance.PlayFootStepSound(transform.position);
                }
            }
        }
    }
}