using System;
using System.Collections.Generic;
using Counters;
using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioClipConfig audioClipConfig;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            DeliveryManager.Instance.OnRecipeSuccess += HandleRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += HandleRecipeFailed;
            CuttingCounter.OnAnyCut += HandleAnyCut;
            Player.Instance.OnPlayerPickupSomething += HandlePlayerPickupSomething;
            BaseCounter.OnAnyObjectPlaced += HandleAnyObjectPlaced;
            TrashCounter.OnAnyObjectTrashed += HandleAnyObjectTrashed;
        }

        private void OnDisable()
        {
            DeliveryManager.Instance.OnRecipeSuccess -= HandleRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed -= HandleRecipeFailed;
            CuttingCounter.OnAnyCut -= HandleAnyCut;
            Player.Instance.OnPlayerPickupSomething -= HandlePlayerPickupSomething;
            BaseCounter.OnAnyObjectPlaced -= HandleAnyObjectPlaced;
            TrashCounter.OnAnyObjectTrashed -= HandleAnyObjectTrashed;
        }

        private void HandleRecipeSuccess(object sender, EventArgs e)
        {
            PlaySound(audioClipConfig.DeliverySuccess, DeliveryManager.Instance.transform.position);
        }

        private void HandleRecipeFailed(object sender, EventArgs e)
        {
            PlaySound(audioClipConfig.DeliveryFailed, DeliveryManager.Instance.transform.position);
        }

        private void HandleAnyCut(object sender, EventArgs e)
        {
            if (sender is CuttingCounter cuttingCounter)
            {
                PlaySound(audioClipConfig.Chop, cuttingCounter.transform.position);
            }
        }

        private void HandlePlayerPickupSomething(object sender, EventArgs e)
        {
            PlaySound(audioClipConfig.ObjectPickup, Player.Instance.transform.position);
        }

        private void HandleAnyObjectPlaced(object sender, EventArgs e)
        {
            if (sender is BaseCounter baseCounter)
            {
                PlaySound(audioClipConfig.ObjectDrop, baseCounter.transform.position);
            }
        }

        private void HandleAnyObjectTrashed(object sender, EventArgs e)
        {
            if (sender is TrashCounter trashCounter)
            {
                PlaySound(audioClipConfig.Trash, trashCounter.transform.position);
            }
        }

        public void PlayFootStepSound(Vector3 position, float volume = 1f)
        {
            PlaySound(audioClipConfig.FootsStep, position, volume);
        }

        private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        private void PlaySound(IReadOnlyList<AudioClip> audioClips, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Count)], position, volume);
        }
    }
}