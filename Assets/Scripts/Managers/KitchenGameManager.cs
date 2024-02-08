using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Managers
{
    public class KitchenGameManager : NetworkStaticInstance<KitchenGameManager>
    {
        public event EventHandler<StateChangedEventArgs> OnStateChanged;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameResume;
        public event EventHandler OnLocalPlayerReadyChanged;

        public class StateChangedEventArgs : EventArgs
        {
            public State state;
        }

        public enum State
        {
            WaitingToStart,
            CountDownToStart,
            GamePlaying,
            GameOver,
        }

        private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
        private float maxWaitingToStartTime = 1f;
        private float maxCountDownToStartTime = 3f;
        private NetworkVariable<float> restCountDownTime = new NetworkVariable<float>(0f);
        private NetworkVariable<float> playingTimerElapsed = new NetworkVariable<float>(0f);
        [SerializeField] private float maxGamePlayTime = 180f;
        private bool isGamePaused;
        private bool isLocalPlayerReady;
        public bool IsLocalPlayerReady => isLocalPlayerReady;
        private Dictionary<ulong, bool> playerReadyDictionary = new Dictionary<ulong, bool>();

        private void OnEnable()
        {
            InputManager.Instance.OnPauseAction += HandlePause;
            InputManager.Instance.OnInteractAction += HandleInteractAction;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnPauseAction -= HandlePause;
            InputManager.Instance.OnInteractAction -= HandleInteractAction;
        }

        private void Start()
        {
            if (IsServer)
            {
                ChangeState(State.WaitingToStart);
            }

            // DebugAutomaticStartGame();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            state.OnValueChanged += HandleStateNetworkChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            state.OnValueChanged -= HandleStateNetworkChanged;
        }

        private void ChangeState(State newState)
        {
            ChangeStateServerRpc(newState);
        }

        [ServerRpc]
        private void ChangeStateServerRpc(State newState)
        {
            switch (newState)
            {
                case State.WaitingToStart:
                    // StartCoroutine(HandleWaitingToStart());
                    break;
                case State.CountDownToStart:
                    StartCoroutine(HandleCountDownToStart());
                    break;
                case State.GamePlaying:
                    StartCoroutine(HandleGamePlaying());
                    break;
                case State.GameOver:
                    break;
            }

            state.Value = newState;
            Debug.Log($"current state = {state.Value.ToString()}");
        }

        private IEnumerator HandleWaitingToStart()
        {
            float timeElapsed = 0f;
            while (timeElapsed < maxWaitingToStartTime)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
            }

            ChangeState(State.CountDownToStart);
        }

        private IEnumerator HandleCountDownToStart()
        {
            restCountDownTime.Value = maxCountDownToStartTime;
            while (restCountDownTime.Value > 0)
            {
                yield return null;
                restCountDownTime.Value -= Time.deltaTime;
            }

            ChangeState(State.GamePlaying);
        }

        private IEnumerator HandleGamePlaying()
        {
            playingTimerElapsed.Value = 0f;
            while (playingTimerElapsed.Value < maxGamePlayTime)
            {
                yield return null;
                playingTimerElapsed.Value += Time.deltaTime;
            }

            ChangeState(State.GameOver);
        }

        private void HandlePause(object sender, EventArgs e)
        {
            TogglePauseGame();
        }

        public void TogglePauseGame()
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                OnGamePaused?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
            }
            else
            {
                OnGameResume?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 1;
            }
        }

        public bool IsGamePlaying => state.Value == State.GamePlaying;
        public bool IsCountingDownToStartActive => state.Value == State.CountDownToStart;
        public float RestCountDownTime => restCountDownTime.Value;
        public float GamePlayingTimeNormalized => playingTimerElapsed.Value / maxGamePlayTime;

        private void HandleInteractAction(object sender, EventArgs e)
        {
            if (state.Value == State.WaitingToStart)
            {
                // ChangeState(State.CountDownToStart);
                isLocalPlayerReady = true;
                OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
                SetPlayerReadyServerRpc();
            }
        }

        private void DebugAutomaticStartGame()
        {
            // maxCountDownToStartTime = 1f;
            ChangeState(State.CountDownToStart);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

            bool allReady = true;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientId) || playerReadyDictionary[clientId] == false)
                {
                    allReady = false;
                    break;
                }
            }

            Debug.Log($"all clients are ready = {allReady}");

            if (allReady)
            {
                ChangeState(State.CountDownToStart);
            }
        }

        private void HandleStateNetworkChanged(State previousValue, State newValue)
        {
            OnStateChanged?.Invoke(this, new StateChangedEventArgs
            {
                state = state.Value
            });
        }
    }
}