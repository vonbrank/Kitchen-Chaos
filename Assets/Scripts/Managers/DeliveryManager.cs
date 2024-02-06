using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KitchenObjects;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class DeliveryManager : NetworkBehaviour
    {
        public event EventHandler OnRecipeSpawned;
        public event EventHandler OnRecipeCompleted;
        public event EventHandler OnRecipeSuccess;
        public event EventHandler OnRecipeFailed;

        public static DeliveryManager Instance { get; private set; }

        [SerializeField] private RecipeList recipeList;
        [SerializeField] private int maxWaitingRecipeAmount = 4;
        [SerializeField] private float maxSpawnRecipeTime = 4;
        private List<RecipeItem> waitingRecipeList = new List<RecipeItem>();

        public IReadOnlyList<RecipeItem> WaitingRecipeList => waitingRecipeList;

        public int SuccessfulRecipesAmount { get; private set; }

        private Coroutine spawnRecipeCoroutine;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            KitchenGameManager.Instance.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            KitchenGameManager.Instance.OnStateChanged -= HandleStateChanged;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (spawnRecipeCoroutine is not null)
            {
                StopCoroutine(spawnRecipeCoroutine);
            }
        }

        private IEnumerator HandleSpawnRecipe()
        {
            if (!IsServer)
            {
                yield break;
            }

            while (true)
            {
                var timeElapsed = 0f;
                while (timeElapsed < maxSpawnRecipeTime)
                {
                    yield return null;
                    timeElapsed += Time.deltaTime;
                }

                if (waitingRecipeList.Count < maxWaitingRecipeAmount)
                {
                    int waitingRecipeItemIndex = Random.Range(0, recipeList.RecipeItemList.Count);
                    SpawnNewWaitingRecipeClientRpc(waitingRecipeItemIndex);
                }
            }
        }

        [ClientRpc]
        private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeItemIndex)
        {
            var waitingRecipeItem = recipeList.RecipeItemList[waitingRecipeItemIndex];
            Debug.Log($"Recipe {waitingRecipeItem.RecipeName} spawned.");
            waitingRecipeList.Add(waitingRecipeItem);

            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }

        public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
        {
            int matchIndex = -1;
            for (int i = 0; i < waitingRecipeList.Count; i++)
            {
                RecipeItem waitingRecipeItem = waitingRecipeList[i];
                if (waitingRecipeItem.KitchenObjectItems.Count == plateKitchenObject.KitchenObjectItems.Count)
                {
                    bool isMatch = true;
                    foreach (KitchenObjectItem kitchenObjectItem in plateKitchenObject.KitchenObjectItems)
                    {
                        if (!waitingRecipeItem.KitchenObjectItems.Contains(kitchenObjectItem))
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        Debug.Log($"Player delivered the correct recipe {waitingRecipeItem.RecipeName}");
                        matchIndex = i;
                        break;
                    }
                }
            }

            if (matchIndex != -1)
            {
                DeliverCorrectRecipeServerRpc(matchIndex);
            }
            else
            {
                DeliverInCorrectRecipeServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DeliverCorrectRecipeServerRpc(int matchIndex)
        {
            DeliverCorrectRecipeClientRpc(matchIndex);
        }

        [ClientRpc]
        private void DeliverCorrectRecipeClientRpc(int matchIndex)
        {
            waitingRecipeList.RemoveAt(matchIndex);

            SuccessfulRecipesAmount++;

            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DeliverInCorrectRecipeServerRpc()
        {
            DeliverInCorrectRecipeClientRpc();
        }

        [ClientRpc]
        private void DeliverInCorrectRecipeClientRpc()
        {
            Debug.LogWarning($"Player delivered a wrong recipe");
            OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        }

        private void HandleStateChanged(object sender, KitchenGameManager.StateChangedEventArgs e)
        {
            if (e.state == KitchenGameManager.State.GamePlaying)
            {
                spawnRecipeCoroutine = StartCoroutine(HandleSpawnRecipe());
            }
        }
    }
}