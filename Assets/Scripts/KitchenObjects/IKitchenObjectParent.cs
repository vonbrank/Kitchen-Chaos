using UnityEngine;

namespace KitchenObjects
{
    public interface IKitchenObjectParent
    {
        public Transform KitchenObjectFollowTransform { get; }

        public KitchenObject KitchenObject { get; }

        public void SetKitchenObject(KitchenObject kitchenObject);

        public void ClearKitchenObject();

        public bool HasKitchenObject();
    }
}