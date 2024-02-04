using System;
using DefaultNamespace;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlaced;

        [SerializeField] private Transform counterTopPoint;

        private KitchenObject kitchenObject;
        public KitchenObject KitchenObject => kitchenObject;

        public virtual void Interact(Player player)
        {
            Debug.LogError("BaseCounter.Interact();");
        }

        public virtual void InteractAlternate(Player player)
        {
            // Debug.LogError("BaseCounter.InteractAlternate();");
        }


        public Transform KitchenObjectFollowTransform => counterTopPoint;

        public void SetKitchenObject(KitchenObject kitchenObject)
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