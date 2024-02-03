using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class ClearCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;

        public override void Interact(Player player)
        {
            if (HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    if (player.KitchenObject is PlateKitchenObject playerPlateKitchenObject)
                    {
                        if (playerPlateKitchenObject.TryAddIngredient(KitchenObject.KitchenObjectItem))
                        {
                            KitchenObject.DestroySelf();
                        }
                    }
                    else if (KitchenObject is PlateKitchenObject plateKitchenObject)
                    {
                        if (plateKitchenObject.TryAddIngredient(player.KitchenObject.KitchenObjectItem))
                        {
                            player.KitchenObject.DestroySelf();
                        }
                    }
                }
                else
                {
                    KitchenObject.SetKitchenObjectParent(player);
                }
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    player.KitchenObject.SetKitchenObjectParent(this);
                }
                else
                {
                }
            }
        }
    }
}