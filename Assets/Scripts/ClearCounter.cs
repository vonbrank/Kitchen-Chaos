using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class ClearCounter : MonoBehaviour
    {
        [FormerlySerializedAs("kitchenObject")] [SerializeField]
        private KitchenObjectItem kitchenObjectItem;

        [SerializeField] private Transform counterTopPoint;

        public void Interact()
        {
            var kitchenObjectTransform = Instantiate(kitchenObjectItem.Prefab, counterTopPoint);
            kitchenObjectTransform.localPosition = Vector3.zero;

            KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            Debug.Log($"kitchen object spawned {kitchenObject.transform}");
        }
    }
}