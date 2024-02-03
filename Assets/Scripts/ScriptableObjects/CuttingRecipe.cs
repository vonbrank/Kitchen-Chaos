using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Cutting Recipe")]
    public class CuttingRecipe : ScriptableObject
    {
        public KitchenObjectItem input;
        public KitchenObjectItem output;
        public int maxCuttingProgress;
    }
}