using System;
using System.Collections;
using DefaultNamespace;
using ScriptableObjects;
using UnityEngine;

namespace Counters
{
    public class PlatesCounter : BaseCounter
    {
        public event EventHandler<PlateSpawnedEventArgs> OnPlateSpawned;

        public class PlateSpawnedEventArgs : EventArgs
        {
            public int currentSpawnedPlatesAmount;
        }

        [SerializeField] private KitchenObjectItem plateKitchenObjectItem;
        private float plateSpawnInterval = 4f;
        private Coroutine currentSpawnPlatesHandler;
        private int currentSpawnedPlatesAmount;
        private int maxSpawnedPlatesAmount = 4;

        private void Start()
        {
            currentSpawnPlatesHandler = StartCoroutine(HandleSpawnPlates());
        }

        private IEnumerator HandleSpawnPlates()
        {
            while (true)
            {
                float timeElapsed = 0;
                while (timeElapsed <= plateSpawnInterval)
                {
                    yield return null;
                    timeElapsed += Time.deltaTime;
                }

                if (currentSpawnedPlatesAmount < maxSpawnedPlatesAmount)
                {
                    currentSpawnedPlatesAmount++;
                    OnPlateSpawned?.Invoke(this, new PlateSpawnedEventArgs
                    {
                        currentSpawnedPlatesAmount = currentSpawnedPlatesAmount
                    });
                    // DefaultNamespace.KitchenObject.SpawnKitchenObject(plateKitchenObjectItem, this);
                }
            }
        }

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                if (currentSpawnedPlatesAmount > 0)
                {
                    currentSpawnedPlatesAmount--;
                    KitchenObject.SpawnKitchenObject(plateKitchenObjectItem, player);
                    OnPlateSpawned?.Invoke(this, new PlateSpawnedEventArgs
                    {
                        currentSpawnedPlatesAmount = currentSpawnedPlatesAmount
                    });
                }
            }
        }
    }
}