using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class KitchenGameLobbyManager : PersistentSingleton<KitchenGameLobbyManager>
    {
        private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
        public event EventHandler OnCreateLobbyStarted;
        public event EventHandler OnCreateLobbyFailed;
        public event EventHandler OnJoinLobbyStarted;
        public event EventHandler OnQuickJoinLobbyFailed;
        public event EventHandler OnJoinLobbyFailed;
        public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

        public class OnLobbyListChangedEventArgs : EventArgs
        {
            public List<Lobby> Lobbies;
        }

        private Lobby joinedLobby;
        private Coroutine hearBeatCoroutine;
        private Coroutine periodListLobbiesCoroutine;

        protected override void Awake()
        {
            base.Awake();

            InitializeUnityAuthentication();
        }

        private void Start()
        {
            hearBeatCoroutine = StartCoroutine(HandleHeartBeat());


            periodListLobbiesCoroutine = StartCoroutine(HandlePeriodListLobbies());
        }

        private void OnDestroy()
        {
            if (hearBeatCoroutine is not null)
            {
                StopCoroutine(hearBeatCoroutine);
            }

            if (periodListLobbiesCoroutine is not null)
            {
                StopCoroutine(periodListLobbiesCoroutine);
            }

            LobbyClearUp();
        }

        private async void InitializeUnityAuthentication()
        {
            if (UnityServices.State == ServicesInitializationState.Initialized)
            {
                return;
            }

            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(Random.Range(0, 1000).ToString());
            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private async Task<Allocation> AllocateRelay()
        {
            try
            {
                Allocation allocation =
                    await RelayService.Instance.CreateAllocationAsync(
                        KitchenGameMultiplayerManager.MAX_PLAYER_COUNT - 1);
                return allocation;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
        }

        private async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return relayJoinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
        }

        private async Task<JoinAllocation> JoinRelay(string joinRelayCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinRelayCode);
                return joinAllocation;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
        }

        public async void CreateLobby(string lobbyName, bool isPrivate)
        {
            OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,
                    KitchenGameMultiplayerManager.MAX_PLAYER_COUNT,
                    new CreateLobbyOptions
                    {
                        IsPrivate = isPrivate
                    });

                Allocation allocation = await AllocateRelay();
                string relayJoinCode = await GetRelayJoinCode(allocation);
                await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) }
                    }
                });
                NetworkManager.Singleton.GetComponent<UnityTransport>()
                    .SetRelayServerData(new RelayServerData(allocation, "dtls"));

                KitchenGameMultiplayerManager.Instance.StartHost();
                SceneLoader.LoadNetwork(SceneLoader.Scene.CharacterSelectScene);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void QuickJoin()
        {
            OnJoinLobbyStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
                NetworkManager.Singleton.GetComponent<UnityTransport>()
                    .SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                KitchenGameMultiplayerManager.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnQuickJoinLobbyFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void JoinWithId(string lobbyId)
        {
            OnJoinLobbyStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
                NetworkManager.Singleton.GetComponent<UnityTransport>()
                    .SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                KitchenGameMultiplayerManager.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnJoinLobbyFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        public async void JoinWithCode(string lobbyCode)
        {
            OnJoinLobbyStarted?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
                NetworkManager.Singleton.GetComponent<UnityTransport>()
                    .SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                KitchenGameMultiplayerManager.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                OnJoinLobbyFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        public Lobby JoinedLobby => joinedLobby;

        private IEnumerator HandleHeartBeat()
        {
            float heartBeatPeriod = 15f;
            while (true)
            {
                if (IsLobbyHost())
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                    Debug.Log("heart beat.");
                }

                yield return new WaitForSeconds(heartBeatPeriod);
            }
        }

        private bool IsLobbyHost()
        {
            return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

        private IEnumerator HandlePeriodListLobbies()
        {
            float refreshPeriod = 3f;
            while (true)
            {
                yield return new WaitForSeconds(refreshPeriod);
                // Debug.Log($"joinedLobby = {joinedLobby}, IsSignedIn = {AuthenticationService.Instance.IsSignedIn}");
                if (joinedLobby is null && AuthenticationService.Instance.IsSignedIn &&
                    SceneManager.GetActiveScene().name == SceneLoader.Scene.LobbyScene.ToString())
                {
                    ListLobbies();
                    // Debug.Log($"Period list lobbies");
                }
            }
        }

        private async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                    }
                };
                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
                {
                    Lobbies = queryResponse.Results
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async void DeleteLobby()
        {
            if (joinedLobby is not null)
            {
                try
                {
                    await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                    joinedLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }

        public async void LeaveLobby()
        {
            if (joinedLobby is not null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id,
                        AuthenticationService.Instance.PlayerId);
                    joinedLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }

        public void LobbyClearUp()
        {
            if (IsLobbyHost())
            {
                DeleteLobby();
            }
            else
            {
                LeaveLobby();
            }
        }

        public async void KickOffPlayer(string playerId)
        {
            if (IsLobbyHost())
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
        }
    }
}