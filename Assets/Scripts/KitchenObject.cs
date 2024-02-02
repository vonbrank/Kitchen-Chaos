using UnityEngine;

namespace DefaultNamespace
{
    public class KitchenObject : MonoBehaviour
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;
        public KitchenObjectItem KitchenObjectItem => kitchenObjectItem;

        private IKitchenObjectParent kitchenObjectParent;

        public IKitchenObjectParent KitchenObjectParent => kitchenObjectParent;

        public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
        {
            if (this.kitchenObjectParent is not null)
            {
                this.kitchenObjectParent.ClearKitchenObject();
            }

            this.kitchenObjectParent = kitchenObjectParent;

            if (kitchenObjectParent.HasKitchenObject())
            {
                Debug.LogError("kitchen object parent already has a kitchen object.");
            }

            kitchenObjectParent.SetKitchenObject(this);

            transform.parent = kitchenObjectParent.KitchenObjectFollowTransform;
            transform.localPosition = Vector3.zero;
        }
    }
}