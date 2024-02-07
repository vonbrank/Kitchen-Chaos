using System;
using System.Collections;
using KitchenObjects;
using Managers;
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
            if (currentSpawnPlatesHandler is not null)
            {
                StopCoroutine(currentSpawnPlatesHandler);
            }

            base.OnDestroy();
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

        public override void Interact(Player.Player player)
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

        private void HandleStateChanged(object sender, KitchenGameManager.StateChangedEventArgs e)
        {
            if (e.state == KitchenGameManager.State.GamePlaying)
            {
                currentSpawnPlatesHandler = StartCoroutine(HandleSpawnPlates());
            }
        }
    }
}