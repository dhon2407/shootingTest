using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI.HUD
{
    public class LevelProgression : MonoBehaviour
    {
        [SerializeField]
        private Text currentLevel;
        [SerializeField]
        private Text nextLevel;
        [SerializeField]
        private Slider progressSlider;

        private int _lastScore;

        private void SetLevelDisplay(int current)
        {
            current = Mathf.Max(1, current);
            currentLevel.text = $"{current:0}";
            nextLevel.text = $"{current + 1:0}";
        }

        public void SetData(int newScore)
        {
            DOTween.Kill(this, true);
            DOTween.To(() => _lastScore, value => UpdateScoreValue(value), newScore, 0.3f).OnComplete(() =>
            {
                _lastScore = newScore;
            }).SetId(this);
        }

        private void UpdateScoreValue(float scoreUpdateValue)
        {
            var mCurrentLevel = (int)Mathf.Floor(25 + Mathf.Sqrt(300 + 100 * scoreUpdateValue / 10f)) / 50;
            var scoreMinCurrentLevel = (Mathf.Pow(50 * mCurrentLevel - 25, 2) - 300) / 10f;
            var scoreNextLevel = (Mathf.Pow(50 * (mCurrentLevel + 1) - 25, 2) - 300) / 10f;

            progressSlider.value = Mathf.InverseLerp(scoreMinCurrentLevel, scoreNextLevel, scoreUpdateValue);
            SetLevelDisplay(mCurrentLevel);
        }
    }
}