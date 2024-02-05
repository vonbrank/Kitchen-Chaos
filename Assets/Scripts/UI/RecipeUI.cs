using System;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RecipeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private Transform iconContainer;
        [SerializeField] private Image iconTemplate;

        private void Start()
        {
            iconTemplate.gameObject.SetActive(false);
        }

        public void SetRecipeItem(RecipeItem recipeItem)
        {
            recipeNameText.text = recipeItem.RecipeName;

            foreach (Transform childTransform in iconContainer)
            {
                if (childTransform == iconTemplate.transform) continue;
                Destroy(childTransform.gameObject);
            }

            foreach (var kitchenObjectItem in recipeItem.KitchenObjectItems)
            {
                var image = Instantiate(iconTemplate, iconContainer);
                image.gameObject.SetActive(true);
                image.sprite = kitchenObjectItem.Sprite;
            }
        }
    }
}