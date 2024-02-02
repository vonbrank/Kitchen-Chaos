using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContainerCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;

        public event EventHandler OnPlayerGrabObject;

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectItem, player);

                OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}