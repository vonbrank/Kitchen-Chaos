using UnityEngine;

namespace DefaultNamespace
{
    public class KitchenObject : MonoBehaviour
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;
        public KitchenObjectItem KitchenObjectItem => kitchenObjectItem;
    }
}