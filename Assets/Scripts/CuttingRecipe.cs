using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Cutting Recipe")]
    public class CuttingRecipe : ScriptableObject
    {
        public KitchenObjectItem input;
        public KitchenObjectItem output;
    }
}