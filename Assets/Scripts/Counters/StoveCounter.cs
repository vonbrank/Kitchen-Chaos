using System;
using System.Collections;
using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class StoveCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<StateChangedEventArgs> OnStateChanged;
        public event EventHandler<IHasProgress.ProgressChangedEventArgs> OnProgressChanged;

        public class StateChangedEventArgs : EventArgs
        {
            public State state;
        }

        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned,
        }

        [SerializeField] private FryingRecipe[] fryingRecipes;
        [SerializeField] private BurningRecipe[] burningRecipes;

        private State currentState;

        private void Start()
        {
            ChangeState(State.Idle);
        }

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

                            ChangeState(State.Idle);
                        }
                    }
                }
                else
                {
                    ChangeState(State.Idle);
                    KitchenObject.SetKitchenObjectParent(player);
                }
            }
            else
            {
                if (player.HasKitchenObject())
                {
                    if (HasFryingRecipeWithInput(player.KitchenObject.KitchenObjectItem))
                    {
                        player.KitchenObject.SetKitchenObjectParent(this);

                        ChangeState(State.Frying);
                    }
                    else if (HasBurningRecipeWithInput(player.KitchenObject.KitchenObjectItem))
                    {
                        player.KitchenObject.SetKitchenObjectParent(this);

                        ChangeState(State.Fried);
                    }
                }
                else
                {
                }
            }
        }


        private bool HasFryingRecipeWithInput(KitchenObjectItem kitchenObjectItem)
        {
            FryingRecipe fryingRecipe = GetFryingRecipeFromInput(kitchenObjectItem);
            return fryingRecipe is not null;
        }

        private KitchenObjectItem GetFryingOutputFromInput(KitchenObjectItem kitchenObjectItem)
        {
            FryingRecipe fryingRecipe = GetFryingRecipeFromInput(kitchenObjectItem);
            if (fryingRecipe is not null)
            {
                return fryingRecipe.output;
            }

            return null;
        }

        private FryingRecipe GetFryingRecipeFromInput(KitchenObjectItem kitchenObjectItem)
        {
            foreach (FryingRecipe fryingRecipe in fryingRecipes)
            {
                if (fryingRecipe.input == kitchenObjectItem)
                {
                    return fryingRecipe;
                }
            }

            return null;
        }

        private bool HasBurningRecipeWithInput(KitchenObjectItem kitchenObjectItem)
        {
            BurningRecipe burningRecipe = GetBurningRecipeFromInput(kitchenObjectItem);
            return burningRecipe is not null;
        }

        private KitchenObjectItem GetBurningOutputFromInput(KitchenObjectItem kitchenObjectItem)
        {
            BurningRecipe burningRecipe = GetBurningRecipeFromInput(kitchenObjectItem);
            if (burningRecipe is not null)
            {
                return burningRecipe.output;
            }

            return null;
        }

        private BurningRecipe GetBurningRecipeFromInput(KitchenObjectItem kitchenObjectItem)
        {
            foreach (BurningRecipe burningRecipe in burningRecipes)
            {
                if (burningRecipe.input == kitchenObjectItem)
                {
                    return burningRecipe;
                }
            }

            return null;
        }

        private Coroutine currentFryHandler;

        private IEnumerator HandleFry()
        {
            if (!HasKitchenObject())
            {
                yield break;
            }

            if (!HasFryingRecipeWithInput(KitchenObject.KitchenObjectItem))
            {
                yield break;
            }

            FryingRecipe fryingRecipe = GetFryingRecipeFromInput(KitchenObject.KitchenObjectItem);

            float timeElapsed = 0;
            while (timeElapsed <= fryingRecipe.maxFryingProgress)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
                {
                    progressNormalized = Mathf.Clamp(timeElapsed / fryingRecipe.maxFryingProgress, 0, 1)
                });
            }

            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);
            ChangeState(State.Fried);
        }

        private Coroutine currentBurningHandler;

        private IEnumerator HandleBurn()
        {
            if (!HasKitchenObject())
            {
                yield break;
            }

            if (!HasBurningRecipeWithInput(KitchenObject.KitchenObjectItem))
            {
                yield break;
            }

            BurningRecipe burningRecipe = GetBurningRecipeFromInput(KitchenObject.KitchenObjectItem);

            float timeElapsed = 0;
            while (timeElapsed <= burningRecipe.maxBurningProgress)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
                {
                    progressNormalized = Mathf.Clamp(timeElapsed / burningRecipe.maxBurningProgress, 0, 1)
                });
            }

            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipe.output, this);
            ChangeState(State.Burned);
        }

        private void ChangeState(State newState)
        {
            if (currentState == State.Idle && newState == State.Frying)
            {
                if (currentFryHandler is not null)
                {
                    StopCoroutine(currentFryHandler);
                }

                currentFryHandler = StartCoroutine(HandleFry());
            }

            if ((currentState == State.Idle || currentState == State.Frying) && newState == State.Fried)
            {
                if (currentBurningHandler is not null)
                {
                    StopCoroutine(currentBurningHandler);
                }

                currentBurningHandler = StartCoroutine(HandleBurn());
            }

            if (newState == State.Idle)
            {
                if (currentFryHandler is not null)
                {
                    StopCoroutine(currentFryHandler);
                }

                if (currentBurningHandler is not null)
                {
                    StopCoroutine(currentBurningHandler);
                }

                OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
                {
                    progressNormalized = 0,
                });
            }

            currentState = newState;
            OnStateChanged?.Invoke(this, new StateChangedEventArgs
            {
                state = currentState
            });
            // Debug.Log($"current state = {currentState.ToString()}");
        }
    }
}