using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Kitchen Object Item")]
    public class KitchenObjectItem : ScriptableObject
    {
        [SerializeField] private Transform prefab;
        public Transform Prefab => prefab;
        [SerializeField] private Sprite sprite;
        [SerializeField] private string objectName;
    }
}