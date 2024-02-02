using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class CuttingCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectItem cuttingKitchenObjectItem;

        public override void Interact(Player player)
        {
            if (HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
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

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject())
            {
                KitchenObject.DestroySelf();

                KitchenObject.SpawnKitchenObject(cuttingKitchenObjectItem, this);
            }
        }
    }
}