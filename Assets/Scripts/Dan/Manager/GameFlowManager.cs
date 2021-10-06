using System;
using System.Collections.Generic;
using System.Linq;
using Dan.Character.Input;
using Dan.UI;
using Dan.UI.HUD;
using UnityEngine;

namespace Dan.Manager
{
    public class GameFlowManager : MonoBehaviour
    {
        [SerializeField]
        private Dan.Character.Player player;
        [SerializeField]
        private TitleScreen titleScreen;
        [SerializeField]
        private HUD hudScreen;
        [SerializeField]
        private PauseScreen pauseScreen;
        [SerializeField]
        private ScoreText scoreText;
        [SerializeField]
        private HealthSlider healthSlider;

        public static event Action OnGameStart;
        public static event Action OnGameEnd;
        private static GameFlowManager _instance;
        private IEnumerable<IInputHandler> _inputHandlers;
        private bool _isPause;
        private bool _gameStarted;
        private int playerScore;

        public static void AddScore(int score)
        {
            _instance.scoreText.Score += score;
            _instance.playerScore = _instance.scoreText.Score;
        }

        private void SetupEvents()
        {
            player.OnUpdateHP += UpdatePlayerHP;
            player.OnPlayerDie += PlayerDie;
            titleScreen.OnPressPlay += StartGame;
            foreach (var inputHandler in _inputHandlers)
                inputHandler.Pause += TogglePause;
        }

        private void ClearEvents()
        {
            player.OnUpdateHP -= UpdatePlayerHP;
            player.OnPlayerDie -= PlayerDie;
            titleScreen.OnPressPlay -= StartGame;
            foreach (var inputHandler in _inputHandlers)
                inputHandler.Pause -= TogglePause;
        }

        private void Start()
        {
            titleScreen.StartNewGame();
        }

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            SetupComponents();
            SetupEvents();
        }

        private void SetupComponents()
        {
            _inputHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IInputHandler>();
        }

        private void TogglePause()
        {
            if (!_gameStarted)
                return;
            
            _isPause = !_isPause;
            
            pauseScreen.Pause(_isPause);
        }

        private void OnDestroy() => ClearEvents();

        private void StartGame()
        {
            titleScreen.Hide();
            hudScreen.Show(0.2f, ()=>
            {
                OnGameStart?.Invoke();
                _gameStarted = true;
            });
        }
        
        private void UpdatePlayerHP(int current, int max)
        {
            healthSlider.SetValue(current / (float)max);
        }
        
        private void PlayerDie()
        {
            _gameStarted = false;
            hudScreen.Hide();
            pauseScreen.EndGame(playerScore);
            OnGameEnd?.Invoke();
        }
    }
}