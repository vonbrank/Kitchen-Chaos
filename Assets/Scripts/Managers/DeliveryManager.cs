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

        private void OnDestroy()
        {
            if (spawnRecipeCoroutine is not null)
            {
                StopCoroutine(spawnRecipeCoroutine);
            }
        }

        private void Start()
        {
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

                SuccessfulRecipesAmount++;

                OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Debug.LogWarning($"Player delivered a wrong recipe");
                OnRecipeFailed?.Invoke(this, EventArgs.Empty);
            }
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