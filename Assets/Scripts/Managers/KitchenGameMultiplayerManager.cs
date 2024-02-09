using System;
using KitchenObjects;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Managers
{
    public class KitchenGameMultiplayerManager : NetworkPersistentSingleton<KitchenGameMultiplayerManager>
    {
        private const int MAX_PLAYER_COUNT = 4;

        public event EventHandler OnTryingToJoinGame;
        public event EventHandler OnFailedToJoinGame;

        [SerializeField] private KitchenObjectList kitchenObjectList;

        public void StartHost()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += HandleConnectionApproval;
            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host...");
        }

        public void StartClient()
        {
            OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnectCallback;
            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client...");
        }

        private void HandleClientDisconnectCallback(ulong obj)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnectCallback;
            OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
        }


        private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest,
            NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            if (SceneManager.GetActiveScene().name != SceneLoader.Scene.CharacterSelectScene.ToString())
            {
                connectionApprovalResponse.Approved = false;
                connectionApprovalResponse.Reason = "Game has already started.";
                return;
            }

            if (NetworkManager.Singleton.ConnectedClientsIds.Count == MAX_PLAYER_COUNT)
            {
                connectionApprovalResponse.Approved = false;
                connectionApprovalResponse.Reason = "Game is full.";
                return;
            }

            connectionApprovalResponse.Approved = true;
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

        public int GetKitchenObjectItemIndex(KitchenObjectItem kitchenObjectItem)
        {
            return kitchenObjectList.KitchenObjectItemList.IndexOf(kitchenObjectItem);
        }

        public KitchenObjectItem GetKitchenObjectItemByIndex(int index)
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