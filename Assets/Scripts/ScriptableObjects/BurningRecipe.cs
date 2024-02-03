using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Burning Recipe")]
    public class BurningRecipe : ScriptableObject
    {
        public KitchenObjectItem input;
        public KitchenObjectItem output;
        public int maxBurningProgress;
    }
}