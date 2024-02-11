using System;
using Unity.Netcode;
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
                KitchenObjects.KitchenObject.DestroyKitchenObject(player.KitchenObject);
                // player.KitchenObject.DestroySelf();
                OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractServerRpc()
        {
            InteractClientRpc();
        }

        [ClientRpc]
        private void InteractClientRpc()
        {

        }
    }
}