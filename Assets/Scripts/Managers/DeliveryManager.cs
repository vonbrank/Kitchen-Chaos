using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class DeliveryManager : MonoBehaviour
    {
        public static DeliveryManager Instance { get; private set; }

        public event EventHandler OnRecipeSpawned;
        public event EventHandler OnRecipeCompleted;
        public event EventHandler OnRecipeSuccess;
        public event EventHandler OnRecipeFailed;

        [SerializeField] private RecipeList recipeList;
        [SerializeField] private int maxWaitingRecipeAmount = 4;
        [SerializeField] private float maxSpawnRecipeTime = 4;
        private List<RecipeItem> waitingRecipeList = new List<RecipeItem>();

        public IReadOnlyList<RecipeItem> WaitingRecipeList => waitingRecipeList;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            StartCoroutine(HandleSpawnRecipe());
        }

        private IEnumerator HandleSpawnRecipe()
        {
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
                    var waitingRecipeItem = recipeList.RecipeItemList[Random.Range(0, recipeList.RecipeItemList.Count)];
                    Debug.Log($"Recipe {waitingRecipeItem.RecipeName} spawned.");
                    waitingRecipeList.Add(waitingRecipeItem);

                    OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
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
                waitingRecipeList.RemoveAt(matchIndex);

                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.LogWarning($"Player delivered a wrong recipe");
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}