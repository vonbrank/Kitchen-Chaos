using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Audio Clip Config")]
    public class AudioClipConfig : ScriptableObject
    {
        [SerializeField] private List<AudioClip> chop;
        public IReadOnlyList<AudioClip> Chop => chop;
        [SerializeField] private List<AudioClip> deliveryFailed;
        public IReadOnlyList<AudioClip> DeliveryFailed => deliveryFailed;
        [SerializeField] private List<AudioClip> deliverySuccess;
        public IReadOnlyList<AudioClip> DeliverySuccess => deliverySuccess;
        [SerializeField] private List<AudioClip> footsStep;
        public IReadOnlyList<AudioClip> FootsStep => footsStep;
        [SerializeField] private List<AudioClip> objectDrop;
        public IReadOnlyList<AudioClip> ObjectDrop => objectDrop;
        [SerializeField] private List<AudioClip> objectPickup;
        public IReadOnlyList<AudioClip> ObjectPickup => objectPickup;
        [SerializeField] private AudioClip stoveSizzle;
        public AudioClip StoveSizzle => stoveSizzle;
        [SerializeField] private List<AudioClip> trash;
        public IReadOnlyList<AudioClip> Trash => trash;
        [SerializeField] private List<AudioClip> warning;
        public IReadOnlyList<AudioClip> Warning => warning;
    }
}