using System;
using Dan.UI;
using Dan.UI.HUD;
using UnityEngine;

namespace Dan.Manager
{
    public class GameFlowManager : MonoBehaviour
    {
        [SerializeField]
        private TitleScreen titleScreen;
        [SerializeField]
        private HUD hudScreen;

        public static event Action OnGameStart;

        private void SetupEvents()
        {
            titleScreen.OnPressPlay += StartGame;
        }
        
        private void ClearEvents()
        {
            titleScreen.OnPressPlay -= StartGame;
        }

        private void Start()
        {
            titleScreen.StartNewGame();
        }

        private void Awake() => SetupEvents();
        private void OnDestroy() => ClearEvents();

        private void StartGame()
        {
            titleScreen.Hide();
            hudScreen.Show(0.2f, OnGameStart.Invoke);
        }
    }
}