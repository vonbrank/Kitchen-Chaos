using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlateKitchenObject : KitchenObject
    {
        public event EventHandler<IngredientAddedEventArgs> OnIngredientAdded;

        public class IngredientAddedEventArgs : EventArgs
        {
            public KitchenObjectItem KitchenObjectItem;
        }

        [SerializeField] private List<KitchenObjectItem> validKitchenObjectItems;

        private List<KitchenObjectItem> kitchenObjectItems = new List<KitchenObjectItem>();

        public bool TryAddIngredient(KitchenObjectItem kitchenObjectItem)
        {
            if (!validKitchenObjectItems.Contains(kitchenObjectItem))
            {
                return false;
            }

            if (kitchenObjectItems.Contains(kitchenObjectItem))
            {
                return false;
            }

            kitchenObjectItems.Add(kitchenObjectItem);
            OnIngredientAdded?.Invoke(this, new IngredientAddedEventArgs
            {
                KitchenObjectItem = kitchenObjectItem
            });
            return true;
        }
    }
}