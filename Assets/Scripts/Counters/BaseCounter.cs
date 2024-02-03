using DefaultNamespace;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
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