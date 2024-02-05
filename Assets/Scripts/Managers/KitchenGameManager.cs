using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class KitchenGameManager : MonoBehaviour
    {
        public static KitchenGameManager Instance { get; private set; }

        public event EventHandler<StateChangedEventArgs> OnStateChanged;
        public event EventHandler OnGamePaused;
        public event EventHandler OnGameResume;

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

        private State state;
        private float maxWaitingToStartTime = 1f;
        private float maxCountDownToStartTime = 3f;
        private float resetCountDownTime;
        private float playingTimerElapsed;
        private float maxGamePlayTime = 20f;
        private bool isGamePaused;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            InputManager.Instance.OnPauseAction += HandlePause;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnPauseAction -= HandlePause;
        }

        private void Start()
        {
            ChangeState(State.WaitingToStart);
        }

        private void ChangeState(State newState)
        {
            switch (newState)
            {
                case State.WaitingToStart:
                    StartCoroutine(HandleWaitingToStart());
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

            state = newState;
            Debug.Log($"current state = {state.ToString()}");
            OnStateChanged?.Invoke(this, new StateChangedEventArgs
            {
                state = state
            });
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
            resetCountDownTime = maxCountDownToStartTime;
            while (resetCountDownTime > 0)
            {
                yield return null;
                resetCountDownTime -= Time.deltaTime;
            }

            ChangeState(State.GamePlaying);
        }

        private IEnumerator HandleGamePlaying()
        {
            playingTimerElapsed = 0f;
            while (playingTimerElapsed < maxGamePlayTime)
            {
                yield return null;
                playingTimerElapsed += Time.deltaTime;
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

        public bool IsGamePlaying => state == State.GamePlaying;
        public bool IsCountingDownToStartActive => state == State.CountDownToStart;
        public float ResetCountDownTime => resetCountDownTime;
        public float GamePlayingTimeNormalized => playingTimerElapsed / maxGamePlayTime;
    }
}