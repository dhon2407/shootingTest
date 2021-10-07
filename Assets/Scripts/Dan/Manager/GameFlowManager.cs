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
        [SerializeField]
        private LevelIndicator levelIndicator;

        public static event Action OnGameStart;
        public static event Action OnGameEnd;
        private static GameFlowManager _instance;
        private IEnumerable<IInputHandler> _inputHandlers;
        private bool _isPause;
        private bool _gameStarted;
        private int _playerScore;
        private int _currentLevel = 1;

        public static int CurrentLevel => _instance._currentLevel;
        
        public static void AddScore(int score)
        {
            _instance.scoreText.Score += score;
            _instance._playerScore = _instance.scoreText.Score;
        }
        
        public static void ReturnToTitle() => _instance.ResetGame();

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

        private void Start() => titleScreen.StartNewGame();
        private void OnDestroy() => ClearEvents();
        
        private void SetupEvents()
        {
            player.OnCharacterHit += UpdatePlayerHP;
            player.OnCharacterDeath += PlayerDie;
            titleScreen.OnPressPlay += StartGame;
            foreach (var inputHandler in _inputHandlers)
                inputHandler.Pause += TogglePause;
        }

        private void ClearEvents()
        {
            player.OnCharacterHit -= UpdatePlayerHP;
            player.OnCharacterDeath -= PlayerDie;
            titleScreen.OnPressPlay -= StartGame;
            foreach (var inputHandler in _inputHandlers)
                inputHandler.Pause -= TogglePause;
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
        
        private void StartGame()
        {
            titleScreen.Hide();
            levelIndicator.Show($"Level {_currentLevel}");
            hudScreen.Show(0.2f, ()=>
            {
                OnGameStart?.Invoke();
                _gameStarted = true;
            });
        }
        
        private void UpdatePlayerHP()
        {
            healthSlider.SetValue( player.HitPoints / (float)player.MaxHitPoints);
        }
        
        private void PlayerDie()
        {
            _gameStarted = false;
            hudScreen.Hide();
            pauseScreen.EndGame(_playerScore);
            OnGameEnd?.Invoke();
        }

        private void ResetGame()
        {
            _gameStarted = false;
            _currentLevel = 1;
            player.ResetGame();
            hudScreen.Hide();
            titleScreen.Show();
            pauseScreen.Hide();
            titleScreen.StartNewGame();
        }
    }
}