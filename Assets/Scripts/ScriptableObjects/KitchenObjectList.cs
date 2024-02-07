using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Kitchen Chaos/Kitchen Object List")]
    public class KitchenObjectList : ScriptableObject
    {
        [SerializeField] private List<KitchenObjectItem> kitchenObjectItemList;
        public List<KitchenObjectItem> KitchenObjectItemList => kitchenObjectItemList;
    }
}