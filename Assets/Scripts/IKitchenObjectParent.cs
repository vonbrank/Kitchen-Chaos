using UnityEngine;

namespace DefaultNamespace
{
    public interface IKitchenObjectParent
    {
        public Transform KitchenObjectFollowTransform { get; }

        public void SetKitchenObject(KitchenObject kitchenObject);

        public void ClearKitchenObject();

        public bool HasKitchenObject();
    }
}