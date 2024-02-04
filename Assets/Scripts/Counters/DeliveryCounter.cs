using DefaultNamespace;
using UnityEngine;

namespace Counters
{
    public class DeliveryCounter : BaseCounter
    {
        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                if (player.KitchenObject is PlateKitchenObject plateKitchenObject)
                {
                    player.KitchenObject.DestroySelf();
                }
            }
        }
    }
}