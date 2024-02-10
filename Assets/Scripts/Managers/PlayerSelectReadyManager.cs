using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Managers
{
    public class PlayerSelectReadyManager : NetworkSingleton<PlayerSelectReadyManager>
    {
        private Dictionary<ulong, bool> playerSelectReadyDictionary = new Dictionary<ulong, bool>();
        public event EventHandler OnPlayerReadyChanged;

        public void PlayerSelectReady()
        {
            PlayerSelectReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerSelectReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerSelectReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
            SelectPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

            bool allReady = true;
            int readyClientCount = 0;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerSelectReadyDictionary.ContainsKey(clientId) ||
                    playerSelectReadyDictionary[clientId] == false)
                {
                    allReady = false;
                }
                else
                {
                    readyClientCount++;
                }
            }

            Debug.Log(
                $"{readyClientCount}/{NetworkManager.Singleton.ConnectedClientsIds.Count} client(s) have selected ready");

            if (allReady)
            {
                SceneLoader.LoadNetwork(SceneLoader.Scene.GameScene);
            }
        }

        [ClientRpc]
        private void SelectPlayerReadyClientRpc(ulong clientId)
        {
            playerSelectReadyDictionary[clientId] = true;
            OnPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsPlayerReady(ulong clientId)
        {
            return playerSelectReadyDictionary.ContainsKey(clientId) && playerSelectReadyDictionary[clientId];
        }
    }
}