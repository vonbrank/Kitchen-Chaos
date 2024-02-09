using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Managers
{
    public class PlayerSelectReadyManager : NetworkSingleton<PlayerSelectReadyManager>
    {
        private Dictionary<ulong, bool> playerReadyDictionary = new Dictionary<ulong, bool>();

        public void PlayerSelectReady()
        {
            PlayerSelectReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerSelectReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            bool allReady = true;
            int readyClientCount = 0;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientId) || playerReadyDictionary[clientId] == false)
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
    }
}