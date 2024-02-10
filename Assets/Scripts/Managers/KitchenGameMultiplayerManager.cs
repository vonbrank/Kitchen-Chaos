using System;
using System.Collections.Generic;
using KitchenObjects;
using Player;
using ScriptableObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utils;

namespace Managers
{
    public class KitchenGameMultiplayerManager : NetworkPersistentSingleton<KitchenGameMultiplayerManager>
    {
        private const int MAX_PLAYER_COUNT = 4;

        public event EventHandler OnTryingToJoinGame;
        public event EventHandler OnFailedToJoinGame;

        public event EventHandler OnPlayerDataNetworkListChanged;

        [SerializeField] private KitchenObjectList kitchenObjectList;
        [SerializeField] private List<Color> playerColorList;
        public IReadOnlyList<Color> PlayerColorList => playerColorList;

        private NetworkList<PlayerData> playerDataNetworkList;

        protected override void Awake()
        {
            base.Awake();

            playerDataNetworkList = new NetworkList<PlayerData>();
        }

        private void OnEnable()
        {
            playerDataNetworkList.OnListChanged += HandlePlayerDataNetworkListChanged;
        }

        private void OnDisable()
        {
            playerDataNetworkList.OnListChanged -= HandlePlayerDataNetworkListChanged;
        }

        public void StartHost()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += HandleConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleStartServerClientDisconnectCallback;
            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host...");
        }


        public void StartClient()
        {
            OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleStartClientClientDisconnectCallback;
            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client...");
        }

        private void HandleStartClientClientDisconnectCallback(ulong obj)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleStartClientClientDisconnectCallback;
            OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
        }


        private void HandleClientConnectedCallback(ulong clientId)
        {
            playerDataNetworkList.Add(new PlayerData
            {
                ClientId = clientId,
                ColorIndex = GetFirstAvailableColorIndex()
            });
        }

        private void HandleStartServerClientDisconnectCallback(ulong clientId)
        {
            int removeIndex = -1;
            for (int i = 0; i < playerDataNetworkList.Count; i++)
            {
                if (playerDataNetworkList[i].ClientId == clientId)
                {
                    removeIndex = i;
                }
            }

            if (removeIndex != -1)
            {
                playerDataNetworkList.RemoveAt(removeIndex);
            }
        }

        private void HandlePlayerDataNetworkListChanged(NetworkListEvent<PlayerData> e)
        {
            OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
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

        public bool IsPlayerConnectedByIndex(int index)
        {
            return index <= playerDataNetworkList.Count;
        }

        public PlayerData GetPlayerDataByIndex(int playerIndex)
        {
            return playerDataNetworkList[playerIndex - 1];
        }

        public Color GetPlayerColor(int colorIndex)
        {
            return playerColorList[colorIndex];
        }

        public PlayerData GetPlayerDataByClientId(ulong clientId)
        {
            foreach (PlayerData playerData in playerDataNetworkList)
            {
                if (playerData.ClientId == clientId)
                {
                    return playerData;
                }
            }

            return default;
        }

        public int GetPlayerDataIndexByClientId(ulong clientId)
        {
            for (int i = 0; i < playerDataNetworkList.Count; i++)
            {
                if (playerDataNetworkList[i].ClientId == clientId)
                {
                    return i;
                }
            }

            return -1;
        }

        public PlayerData GetLocalPlayerData()
        {
            return GetPlayerDataByClientId(NetworkManager.Singleton.LocalClientId);
        }


        private bool IsColorIndexAvailable(int colorIndex)
        {
            foreach (PlayerData playerData in playerDataNetworkList)
            {
                if (playerData.ColorIndex == colorIndex)
                {
                    return false;
                }
            }

            return true;
        }

        private int GetFirstAvailableColorIndex()
        {
            for (int i = 0; i < PlayerColorList.Count; i++)
            {
                if (IsColorIndexAvailable(i))
                {
                    return i;
                }
            }

            return -1;
        }

        public void ChangeLocalPlayerColor(int newColorIndex)
        {
            ChangePlayerColorServerRpc(newColorIndex);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangePlayerColorServerRpc(int newColorIndex, ServerRpcParams serverRpcParams = default)
        {
            if (!IsColorIndexAvailable(newColorIndex))
            {
                return;
            }

            int playerDataIndex = GetPlayerDataIndexByClientId(serverRpcParams.Receive.SenderClientId);
            PlayerData newPlayerData = playerDataNetworkList[playerDataIndex];
            newPlayerData.ColorIndex = newColorIndex;
            playerDataNetworkList[playerDataIndex] = newPlayerData;
        }

        public void KickOffPlayer(ulong clientId)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            HandleStartServerClientDisconnectCallback(clientId);
        }
    }
}