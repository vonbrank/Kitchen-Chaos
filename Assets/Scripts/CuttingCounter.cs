using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class CuttingCounter : BaseCounter
    {
        [SerializeField] private CuttingRecipe[] cuttingRecipes;

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
                    if (HasRecipeWithInput(player.KitchenObject.KitchenObjectItem))
                    {
                        player.KitchenObject.SetKitchenObjectParent(this);
                    }
                }
                else
                {
                }
            }
        }

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject() && HasRecipeWithInput(KitchenObject.KitchenObjectItem))
            {
                KitchenObjectItem outputKitchenObjectItem = GetOutputFromInput(KitchenObject.KitchenObjectItem);
                KitchenObject.DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectItem, this);
            }
        }

        private bool HasRecipeWithInput(KitchenObjectItem kitchenObjectItem)
        {
            return GetOutputFromInput(kitchenObjectItem) is not null;
        }

        private KitchenObjectItem GetOutputFromInput(KitchenObjectItem kitchenObjectItem)
        {
            foreach (CuttingRecipe cuttingRecipe in cuttingRecipes)
            {
                if (cuttingRecipe.input == kitchenObjectItem)
                {
                    return cuttingRecipe.output;
                }
            }

            return null;
        }
    }
}