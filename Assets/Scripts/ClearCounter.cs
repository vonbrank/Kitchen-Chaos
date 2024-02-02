using UnityEngine;

namespace DefaultNamespace
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