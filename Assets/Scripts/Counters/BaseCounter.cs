using System;
using KitchenObjects;
using Unity.Netcode;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlaced;

        [SerializeField] private Transform counterTopPoint;

        private KitchenObject kitchenObject;
        public KitchenObject KitchenObject => kitchenObject;

        public virtual void Interact(Player.Player player)
        {
            Debug.LogError("BaseCounter.Interact();");
        }

        public virtual void InteractAlternate(Player.Player player)
        {
            // Debug.LogError("BaseCounter.InteractAlternate();");
        }


        public Transform KitchenObjectFollowTransform => counterTopPoint;

        public virtual void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;
            if (this.kitchenObject)
            {
                OnAnyObjectPlaced?.Invoke(this, EventArgs.Empty);
            }
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return kitchenObject is not null;
        }
    }
}