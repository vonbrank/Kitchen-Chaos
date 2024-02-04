using System;
using DefaultNamespace;
using Managers;
using UnityEngine;

namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public static DeliveryCounter Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                if (player.KitchenObject is PlateKitchenObject plateKitchenObject)
                {
                    DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                    player.KitchenObject.DestroySelf();
                }
            }
        }
    }
}