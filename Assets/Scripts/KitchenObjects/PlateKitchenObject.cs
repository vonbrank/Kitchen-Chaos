using System;
using System.Collections.Generic;
using Managers;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace KitchenObjects
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

        public IReadOnlyList<KitchenObjectItem> KitchenObjectItems => kitchenObjectItems;

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

            var kitchenObjectItemIndex =
                KitchenGameMultiplayerManager.Instance.GetKitchenObjectItemIndex(kitchenObjectItem);
            AddIngredientServerRpc(kitchenObjectItemIndex);

            return true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddIngredientServerRpc(int kitchenObjectItemIndex)
        {
            AddIngredientClientRpc(kitchenObjectItemIndex);
        }

        [ClientRpc]
        private void AddIngredientClientRpc(int kitchenObjectItemIndex)
        {
            var kitchenObjectItem =
                KitchenGameMultiplayerManager.Instance.GetKitchenObjectItemByIndex(kitchenObjectItemIndex);
            kitchenObjectItems.Add(kitchenObjectItem);
            OnIngredientAdded?.Invoke(this, new IngredientAddedEventArgs
            {
                KitchenObjectItem = kitchenObjectItem
            });
        }
    }
}