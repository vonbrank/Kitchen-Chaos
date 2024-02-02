using UnityEngine;

namespace DefaultNamespace
{
    public class ClearCounter : BaseCounter
    {
        [SerializeField] private KitchenObjectItem kitchenObjectItem;

        public override void Interact(Player player)
        {
        }
    }
}