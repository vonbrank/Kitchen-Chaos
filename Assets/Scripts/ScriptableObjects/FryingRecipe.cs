using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Frying Recipe")]
    public class FryingRecipe : ScriptableObject
    {
        public KitchenObjectItem input;
        public KitchenObjectItem output;
        public int maxFryingProgress;
    }
}