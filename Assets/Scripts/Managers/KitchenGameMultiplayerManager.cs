using System;
using KitchenObjects;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;

namespace Managers
{
    public class KitchenGameMultiplayerManager : NetworkBehaviour
    {
        public static KitchenGameMultiplayerManager Instance { get; private set; }

        [SerializeField] private KitchenObjectList kitchenObjectList;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }


        public void SpawnKitchenObject(KitchenObjectItem kitchenObjectItem,
            IKitchenObjectParent kitchenObjectParent)
        {
            int kitchenObjectItemIndex = GetKitchenObjectItemIndex(kitchenObjectItem);
            // Debug.Log($"SpawnKitchenObject {kitchenObjectItem.name}");
            SpawnKitchenObjectServerRpc(kitchenObjectItemIndex, kitchenObjectParent.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnKitchenObjectServerRpc(int kitchenObjectItemIndex,
            NetworkObjectReference kitchenObjectParentNetworkObjectReference)
        {
            var kitchenObjectItem = GetKitchenObjectItemByIndex(kitchenObjectItemIndex);
            // Debug.Log($"SpawnKitchenObjectServerRpc {kitchenObjectItem.name}");
            var kitchenObjectTransform = Instantiate(kitchenObjectItem.Prefab);

            NetworkObject networkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
            networkObject.Spawn(true);

            var kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

            kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
            IKitchenObjectParent kitchenObjectParent =
                kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
            // return kitchenObject;
        }

        private int GetKitchenObjectItemIndex(KitchenObjectItem kitchenObjectItem)
        {
            return kitchenObjectList.KitchenObjectItemList.IndexOf(kitchenObjectItem);
        }

        private KitchenObjectItem GetKitchenObjectItemByIndex(int index)
        {
            return kitchenObjectList.KitchenObjectItemList[index];
        }

        public void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
        {
            kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
            KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

            ClearKitchenObjectOnParentClientRpc(kitchenObjectNetworkObjectReference);
            
            kitchenObject.DestroySelf();
        }

        [ClientRpc]
        private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
        {
            kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
            KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();
            
            kitchenObject.ClearKitchenObjectOnParent();
        }
    }
}