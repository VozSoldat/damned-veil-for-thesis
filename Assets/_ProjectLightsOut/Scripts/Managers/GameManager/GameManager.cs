using System;
using System.Collections;
using ProjectLightsOut.DevUtils;
using UnityEngine;

namespace ProjectLightsOut.Managers
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : Singleton<GameManager>
    {
        private Coroutine resetTimeScaleCoroutine;
        private GameState gameState = GameState.Playing;
        private Action OnSceneLoadComplete;
        private bool isPaused = false;
        public static bool IsPaused => Instance.isPaused;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            EventManager.AddListener<OnSlowTime>(OnSlowTimeEvent);
            EventManager.AddListener<OnChangeGameState>(OnChangeGameState);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<OnSlowTime>(OnSlowTimeEvent);
            EventManager.RemoveListener<OnChangeGameState>(OnChangeGameState);
        }

        private void Start()
        {
            if (LevelManager.Instance == null)
            {
                EventManager.Broadcast(new OnPlayBGM("MainMenu", fadeIn: 10f));
            }
        }

        private void Update()
        {
            if (gameState != GameState.Playing) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        public void OnChangeGameState(OnChangeGameState evt)
        {
            switch (evt.GameState)
            {
                case GameState.Playing:
                    EventManager.Broadcast(new OnResetScore());
                    EventManager.Broadcast(new OnPlayBGM("Gameplay"));
                    Cursor.visible = false;
                    break;
                case GameState.GameOver:
                    EventManager.Broadcast(new OnPlayBGM("GameOver"));
                    Cursor.visible = true;
                    break;
                case GameState.Paused:
                    break;
            }

            gameState = evt.GameState;
        }

        private void OnSlowTimeEvent(OnSlowTime evt)
        {
            SlowTime(evt.TimeScale, evt.Duration);
        }

        public static void SlowTime(float timeScale, float duration)
        {
            if (Instance.resetTimeScaleCoroutine != null)
            {
                Instance.StopCoroutine(Instance.resetTimeScaleCoroutine);
                Time.timeScale = 1f;
            }

            Time.timeScale = timeScale;
            Instance.resetTimeScaleCoroutine = Instance.StartCoroutine(Instance.ResetTimeScale(duration));
        }

        private IEnumerator ResetTimeScale(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }

        #region Pause
        private void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            EventManager.Broadcast(new OnPause(isPaused));
            EventManager.Broadcast(new OnPlayerEnableShooting(!isPaused));
            Cursor.visible = isPaused;
        }

        public void RestartGame()
        {
            EventManager.Broadcast(new OnChangeGameState(GameState.Playing));
            isPaused = false;
            Time.timeScale = 1f;
        }

        public void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            gameState = GameState.Playing;
            EventManager.Broadcast(new OnPause(false));
            EventManager.Broadcast(new OnPlayerEnableShooting(true));
            Cursor.visible = false;
        }

        public void QuitToMainMenu()
        {
            // Reset game state before transitioning to main menu
            isPaused = false;
            Time.timeScale = 1f;
            gameState = GameState.Playing;
            EventManager.Broadcast(new OnPause(false));
            EventManager.Broadcast(new OnPlayerEnableShooting(true));
            
            // Notify that we're leaving gameplay
            EventManager.Broadcast(new OnChangeGameState(GameState.GameOver));
        }
            
        #endregion

    }
}