using System;
using System.Collections.Generic;
using UnityEngine;

namespace Counters
{
    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform counterTopPoint;
        [SerializeField] private Transform plateVisualPrefab;

        private List<GameObject> plateVisualGameObjects = new List<GameObject>();
        float plateOffsetY = 0.1f;

        private void OnEnable()
        {
            platesCounter.OnPlateSpawned += HandlePlateSpawn;
        }

        private void OnDisable()
        {
            platesCounter.OnPlateSpawned -= HandlePlateSpawn;
        }

        private void HandlePlateSpawn(object sender, PlatesCounter.PlateSpawnedEventArgs eventArgs)
        {
            int startIndex = plateVisualGameObjects.Count;
            int endIndex = eventArgs.currentSpawnedPlatesAmount;
            for (int i = startIndex; i < endIndex; i++)
            {
                var plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

                plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjects.Count, 0);
                plateVisualGameObjects.Add(plateVisualTransform.gameObject);
            }

            for (int i = 0; i < plateVisualGameObjects.Count; i++)
            {
                plateVisualGameObjects[i].SetActive(i < eventArgs.currentSpawnedPlatesAmount);
            }
        }
    }
}