using DefaultNamespace;
using UnityEngine;

namespace Counters
{
    public class TrashCounter : BaseCounter
    {
        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                player.KitchenObject.DestroySelf();
            }
        }
    }
}