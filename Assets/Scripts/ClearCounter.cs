using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class ClearCounter : MonoBehaviour, IKitchenObjectParent
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;

        [SerializeField] private Transform counterTopPoint;
        public Transform KitchenObjectFollowTransform => counterTopPoint;

        private KitchenObject kitchenObject;

        public KitchenObject KitchenObject => kitchenObject;

        public void Interact(Player player)
        {
            if (kitchenObject is null)
            {
                var kitchenObjectTransform = Instantiate(kitchenObjectItem.Prefab, counterTopPoint);
                var newKitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
                newKitchenObject.SetKitchenObjectParent(this);
            }
            else
            {
                kitchenObject.SetKitchenObjectParent(player);
            }
        }

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