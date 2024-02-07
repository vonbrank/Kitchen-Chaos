using System;
using System.Collections;
using KitchenObjects;
using ScriptableObjects;
using Unity.Netcode;
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

        private NetworkVariable<State> currentState = new NetworkVariable<State>(State.Idle);
        private NetworkVariable<float> progressNormalized = new NetworkVariable<float>(0f);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            currentState.OnValueChanged += HandleCurrentStateNetworkValueChanged;
            progressNormalized.OnValueChanged += HandleProgressNormalizedNetworkValueChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            currentState.OnValueChanged -= HandleCurrentStateNetworkValueChanged;
            progressNormalized.OnValueChanged -= HandleProgressNormalizedNetworkValueChanged;
        }

        private void Start()
        {
            if (IsServer)
            {
                ChangeState(State.Idle);
            }
        }

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
                            KitchenObject.DestroyKitchenObject(KitchenObject);
                            // KitchenObject.DestroySelf();

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

            float fryTimeElapsed = 0;
            while (fryTimeElapsed <= fryingRecipe.maxFryingProgress)
            {
                yield return null;
                fryTimeElapsed += Time.deltaTime;
                progressNormalized.Value = Mathf.Clamp(fryTimeElapsed / fryingRecipe.maxFryingProgress, 0, 1);
                // OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
                // {
                //     progressNormalized = Mathf.Clamp(fryTimeElapsed / fryingRecipe.maxFryingProgress, 0, 1)
                // });
            }

            KitchenObject.DestroyKitchenObject(KitchenObject);
            // KitchenObject.DestroySelf();
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

            float burnTimeElapsed = 0;
            while (burnTimeElapsed <= burningRecipe.maxBurningProgress)
            {
                yield return null;
                burnTimeElapsed += Time.deltaTime;
                progressNormalized.Value = Mathf.Clamp(burnTimeElapsed / burningRecipe.maxBurningProgress, 0, 1);
                // OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
                // {
                //     progressNormalized = Mathf.Clamp(burnTimeElapsed / burningRecipe.maxBurningProgress, 0, 1)
                // });
            }

            KitchenObject.DestroyKitchenObject(KitchenObject);
            // KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipe.output, this);
            ChangeState(State.Burned);
            progressNormalized.Value = 1;
            // OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
            // {
            //     progressNormalized = 1
            // });
        }

        private void ChangeState(State newState)
        {
            ChangeStateServerRpc(newState);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangeStateServerRpc(State newState)
        {
            if (currentState.Value == State.Idle && newState == State.Frying)
            {
                if (currentFryHandler is not null)
                {
                    StopCoroutine(currentFryHandler);
                }

                currentFryHandler = StartCoroutine(HandleFry());
            }

            if ((currentState.Value == State.Idle || currentState.Value == State.Frying) && newState == State.Fried)
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

                progressNormalized.Value = 0;
                // OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
                // {
                //     progressNormalized = 0,
                // });
            }

            currentState.Value = newState;

            // Debug.Log($"current state = {currentState.ToString()}");
        }

        private void HandleCurrentStateNetworkValueChanged(State previousValue, State newValue)
        {
            // Debug.Log($"stove counter current state = {newValue.ToString()}");
            OnStateChanged?.Invoke(this, new StateChangedEventArgs
            {
                state = newValue
            });
        }

        private void HandleProgressNormalizedNetworkValueChanged(float previousValue, float newValue)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.ProgressChangedEventArgs
            {
                progressNormalized = newValue,
            });
        }


        public bool IsFried => currentState.Value == State.Fried;
    }
}