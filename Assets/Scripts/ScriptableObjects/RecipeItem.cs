using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Recipe Item")]
    public class RecipeItem : ScriptableObject
    {
        [SerializeField] private List<KitchenObjectItem> kitchenObjectItems;
        [SerializeField] private string recipeName;
        public string RecipeName => recipeName;
        public IReadOnlyList<KitchenObjectItem> KitchenObjectItems => kitchenObjectItems;
    }
}