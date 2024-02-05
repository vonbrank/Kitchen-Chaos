using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace KitchenObjects
{
    public class PlateCompleteVisual : MonoBehaviour
    {
        [Serializable]
        public struct KitchenObjectItemGameObject
        {
            public KitchenObjectItem KitchenObjectItem;
            public GameObject GameObject;
        }

        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private List<KitchenObjectItemGameObject> kitchenObjectItemGameObjects;

        private void Start()
        {
            foreach (KitchenObjectItemGameObject kitchenObjectItemGameObject in kitchenObjectItemGameObjects)
            {
                kitchenObjectItemGameObject.GameObject.SetActive(false);
            }
        }

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
            foreach (KitchenObjectItemGameObject kitchenObjectItemGameObject in kitchenObjectItemGameObjects)
            {
                if (kitchenObjectItemGameObject.KitchenObjectItem == eventArgs.KitchenObjectItem)
                {
                    kitchenObjectItemGameObject.GameObject.SetActive(true);
                }
            }
        }
    }
}