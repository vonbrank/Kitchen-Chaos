using System;
using ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace
{
    public class DeliveryManagerUI : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private RecipeUI recipeTemplate;

        private void Awake()
        {
            recipeTemplate.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            DeliveryManager.Instance.OnRecipeSpanwed += HandleRecipeSpawned;
            DeliveryManager.Instance.OnRecipeCompleted += HandleRecipeCompleted;
        }

        private void OnDisable()
        {
            DeliveryManager.Instance.OnRecipeSpanwed -= HandleRecipeSpawned;
            DeliveryManager.Instance.OnRecipeCompleted -= HandleRecipeCompleted;
        }

        private void HandleRecipeSpawned(object sender, EventArgs e)
        {
            UpdateVisual();
        }

        private void HandleRecipeCompleted(object sender, EventArgs e)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (Transform childTransform in container)
            {
                if (childTransform == recipeTemplate.transform) continue;
                Destroy(childTransform.gameObject);
            }

            foreach (RecipeItem recipeItem in DeliveryManager.Instance.WaitingRecipeList)
            {
                var recipeUI = Instantiate(recipeTemplate, container);
                recipeUI.gameObject.SetActive(true);
                recipeUI.SetRecipeItem(recipeItem);
            }
        }
    }
}