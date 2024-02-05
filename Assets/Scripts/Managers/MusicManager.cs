using UnityEngine;

namespace Managers
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }
        private float volumeMultiplier = 0.5f;
        public float VolumeMultiplier => volumeMultiplier;

        private AudioSource audioSource;
        private float volume;

        private const string PLAYER_REFS_MUSIC_VOLUME_MULTIPLIER = "MusicVolumeMultiplier";

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

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