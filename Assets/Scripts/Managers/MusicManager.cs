using UnityEngine;
using Utils;

namespace Managers
{
    public class MusicManager : StaticInstance<MusicManager>
    {
        private float volumeMultiplier = 0.5f;
        public float VolumeMultiplier => volumeMultiplier;

        private AudioSource audioSource;
        private float volume;

        private const string PLAYER_REFS_MUSIC_VOLUME_MULTIPLIER = "MusicVolumeMultiplier";

        protected override void Awake()
        {
            base.Awake();

            audioSource = GetComponent<AudioSource>();
            volume = audioSource.volume;

            volumeMultiplier = PlayerPrefs.GetFloat(PLAYER_REFS_MUSIC_VOLUME_MULTIPLIER, 0.5f);

            audioSource.volume = volumeMultiplier * volume;
        }

        public void ChangeVolume()
        {
            volumeMultiplier += 0.1f;
            if (volumeMultiplier > 1f)
            {
                volumeMultiplier = 0;
            }

            PlayerPrefs.SetFloat(PLAYER_REFS_MUSIC_VOLUME_MULTIPLIER, volumeMultiplier);
            PlayerPrefs.Save();

            audioSource.volume = volumeMultiplier * volume;
        }
    }
}