using KitchenObjects;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlateIconsUI : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private Transform iconTemplate;

        private void OnEnable()
        {
            plateKitchenObject.OnIngredientAdded += HandleIngredientAdded;
        }

        private void OnDisable()
        {
            plateKitchenObject.OnIngredientAdded -= HandleIngredientAdded;
        }

        private void HandleIngredientAdded(object sender, PlateKitchenObject.IngredientAddedEventArgs eventArgs)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (Transform childTransform in transform)
            {
                if (childTransform == iconTemplate) continue;
                Destroy(childTransform.gameObject);
            }

            foreach (KitchenObjectItem kitchenObjectItem in plateKitchenObject.KitchenObjectItems)
            {
                var iconTransform = Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);
                var plateIconImageUI = iconTransform.GetComponentInChildren<PlateIconImageUI>();
                plateIconImageUI.SetKitchenObjectItem(kitchenObjectItem);
            }
        }
    }
}