using System;
using KitchenObjects;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event EventHandler OnAnyCut;
        public event EventHandler<IHasProgress.ProgressChangedEventArgs> OnProgressChanged;

        public event EventHandler OnCut;

        [SerializeField] private CuttingRecipe[] cuttingRecipes;

        private int cuttingProgress;

        public override void Interact(Player.Player player)
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
                    }
                }
                else
                {
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CutObjectServerRpc()
        {
            CutObjectClientRpc();
        }

        [ClientRpc]
        private void CutObjectClientRpc()
        {
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);
            SetCuttingProgress(cuttingProgress + 1);
        }

        [ServerRpc(RequireOwnership = false)]
        private void TestCuttingProgressDoneServerRpc()
        {
            CuttingRecipe cuttingRecipe = GetCuttingRecipeFromInput(KitchenObject.KitchenObjectItem);
            if (cuttingProgress >= cuttingRecipe.maxCuttingProgress)
            {
                KitchenObjectItem outputKitchenObjectItem = GetOutputFromInput(KitchenObject.KitchenObjectItem);

                KitchenObject.DestroyKitchenObject(KitchenObject);
                // KitchenObject.DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectItem, this);
            }
        }

        public override void InteractAlternate(Player.Player player)
        {
            if (HasKitchenObject() && HasRecipeWithInput(KitchenObject.KitchenObjectItem))
            {
                CutObjectServerRpc();
                TestCuttingProgressDoneServerRpc();
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
            float maxCuttingProgress = 1;
            if (KitchenObject)
            {
                CuttingRecipe cuttingRecipe = GetCuttingRecipeFromInput(KitchenObject.KitchenObjectItem);
                if (cuttingRecipe)
                {
                    maxCuttingProgress = cuttingRecipe.maxCuttingProgress;
                }
            }

            cuttingProgress = newProgress;
            OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / maxCuttingProgress
            });
        }

        public override void SetKitchenObject(KitchenObject kitchenObject)
        {
            base.SetKitchenObject(kitchenObject);

            SetCuttingProgress(0);
        }
    }
}