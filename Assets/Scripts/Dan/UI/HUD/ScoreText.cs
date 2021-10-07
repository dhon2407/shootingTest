using System;
using Dan.Manager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI.HUD
{
    [RequireComponent(typeof(Text))]
    public class ScoreText : MonoBehaviour
    {
        public int Score
        {
            get => _currentValue;
            set
            {
                var newScore = Mathf.Min(value, 99999999);
                UpdateScore(_currentValue, newScore);
                _currentValue = newScore;
            }
        }

        private Text _score;
        private int _currentValue = 0;

        private void Awake()
        {
            _score = GetComponent<Text>();
            _score.text = $"Score {_currentValue:00000000}";

            GameFlowManager.OnGameStart += NewGameScore;
        }

        private void OnDestroy()
        {
            GameFlowManager.OnGameStart -= NewGameScore;
        }

        private void NewGameScore()
        {
            _currentValue = 0;
            _score.text = $"Score {_currentValue:00000000}";
        }

        private void UpdateScore(int oldScore, int newScore)
        {
            transform.DOKill(true);
            transform.DOPunchScale(Vector3.one * .05f, .1f, 6, .68f);
            
            DOTween.To(() => oldScore, value => _score.text = $"Score {value:00000000}", newScore, 0.5f);
        }
    }
}