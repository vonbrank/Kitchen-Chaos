using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Recipe List")]
    public class RecipeList : ScriptableObject
    {
        [SerializeField] private List<RecipeItem> recipeItemList;
        public IReadOnlyList<RecipeItem> RecipeItemList => recipeItemList;
    }
}