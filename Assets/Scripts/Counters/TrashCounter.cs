using System;
using UnityEngine;

namespace Counters
{
    public class TrashCounter : BaseCounter
    {
        public static event EventHandler OnAnyObjectTrashed;

        public override void Interact(Player.Player player)
        {
            if (player.HasKitchenObject())
            {
                player.KitchenObject.DestroySelf();
                OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}