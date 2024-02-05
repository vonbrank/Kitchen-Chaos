using System;
using KitchenObjects;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class ContainerCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;

        public event EventHandler OnPlayerGrabObject;

        public override void Interact(Player.Player player)
        {
            if (!player.HasKitchenObject())
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectItem, player);

                OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}