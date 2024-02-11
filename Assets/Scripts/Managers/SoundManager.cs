using System;
using System.Collections.Generic;
using Counters;
using ScriptableObjects;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SoundManager : StaticInstance<SoundManager>
    {
        [SerializeField] private AudioClipConfig audioClipConfig;

        private float volumeMultiplier = 0.5f;
        public float VolumeMultiplier => volumeMultiplier;

        private const string PLAYER_REFS_SOUND_EFFECTS_VOLUME_MULTIPLIER = "SoundEffectsVolumeMultiplier";

        protected override void Awake()
        {
            base.Awake();

            volumeMultiplier = PlayerPrefs.GetFloat(PLAYER_REFS_SOUND_EFFECTS_VOLUME_MULTIPLIER, 0.5f);
        }

        private void OnEnable()
        {
            DeliveryManager.Instance.OnRecipeSuccess += HandleRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += HandleRecipeFailed;
            CuttingCounter.OnAnyCut += HandleAnyCut;
            Player.Player.OnAnyPlayerPickupSomething += HandlePlayerPickupSomething;
            BaseCounter.OnAnyObjectPlaced += HandleAnyObjectPlaced;
            TrashCounter.OnAnyObjectTrashed += HandleAnyObjectTrashed;
        }

        private void OnDisable()
        {
            DeliveryManager.Instance.OnRecipeSuccess -= HandleRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed -= HandleRecipeFailed;
            CuttingCounter.OnAnyCut -= HandleAnyCut;
            Player.Player.OnAnyPlayerPickupSomething -= HandlePlayerPickupSomething;
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
            if (sender is Player.Player player)
            {
                PlaySound(audioClipConfig.ObjectPickup, player.transform.position);
            }
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

        public void PlayCountDownSound()
        {
            PlaySound(audioClipConfig.Warning, Vector3.zero);
        }

        public void PlayWarningSound(Vector3 position)
        {
            PlaySound(audioClipConfig.Warning, position);
        }

        private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
        }

        private void PlaySound(IReadOnlyList<AudioClip> audioClips, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClips[Random.Range(0, audioClips.Count)], position,
                volume * volumeMultiplier);
        }

        public void ChangeVolume()
        {
            volumeMultiplier += 0.1f;
            if (volumeMultiplier > 1f)
            {
                volumeMultiplier = 0;
            }

            PlayerPrefs.SetFloat(PLAYER_REFS_SOUND_EFFECTS_VOLUME_MULTIPLIER, volumeMultiplier);
            PlayerPrefs.Save();
        }
    }
}