using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PlateIconImageUI : MonoBehaviour
    {
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void SetKitchenObjectItem(KitchenObjectItem kitchenObjectItem)
        {
            image.sprite = kitchenObjectItem.Sprite;
        }
    }
}