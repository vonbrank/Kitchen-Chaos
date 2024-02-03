using System;
using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<IHasProgress.ProgressChangedEventArgs> OnProgressChanged;

        public event EventHandler OnCut;

        [SerializeField] private CuttingRecipe[] cuttingRecipes;

        private int cuttingProgress;

        public override void Interact(Player player)
        {
            if (HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    if (player.KitchenObject is PlateKitchenObject plateKitchenObject)
                    {
                        if (plateKitchenObject.TryAddIngredient(KitchenObject.KitchenObjectItem))
                        {
                            KitchenObject.DestroySelf();
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
                    if (HasRecipeWithInput(player.KitchenObject.KitchenObjectItem))
                    {
                        player.KitchenObject.SetKitchenObjectParent(this);

                        SetCuttingProgress(0);
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
                CuttingRecipe cuttingRecipe = GetCuttingRecipeFromInput(KitchenObject.KitchenObjectItem);

                OnCut?.Invoke(this, EventArgs.Empty);
                SetCuttingProgress(cuttingProgress + 1);

                if (cuttingProgress >= cuttingRecipe.maxCuttingProgress)
                {
                    KitchenObjectItem outputKitchenObjectItem = GetOutputFromInput(KitchenObject.KitchenObjectItem);
                    KitchenObject.DestroySelf();

                    KitchenObject.SpawnKitchenObject(outputKitchenObjectItem, this);
                }
            }
        }

        private bool HasRecipeWithInput(KitchenObjectItem kitchenObjectItem)
        {
            CuttingRecipe cuttingRecipe = GetCuttingRecipeFromInput(kitchenObjectItem);
            return cuttingRecipe is not null;
        }

        private KitchenObjectItem GetOutputFromInput(KitchenObjectItem kitchenObjectItem)
        {
            CuttingRecipe cuttingRecipe = GetCuttingRecipeFromInput(kitchenObjectItem);
            if (cuttingRecipe is not null)
            {
                return cuttingRecipe.output;
            }

            return null;
        }

        private CuttingRecipe GetCuttingRecipeFromInput(KitchenObjectItem kitchenObjectItem)
        {
            foreach (CuttingRecipe cuttingRecipe in cuttingRecipes)
            {
                if (cuttingRecipe.input == kitchenObjectItem)
                {
                    return cuttingRecipe;
                }
            }

            return null;
        }

        private void SetCuttingProgress(int newProgress)
        {
            CuttingRecipe cuttingRecipe = GetCuttingRecipeFromInput(KitchenObject.KitchenObjectItem);
            cuttingProgress = newProgress;
            OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipe.maxCuttingProgress
            });
        }
    }
}