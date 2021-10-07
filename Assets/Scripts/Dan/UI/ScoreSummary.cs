using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI
{
    public class ScoreSummary : MonoBehaviour
    {
        private string HighScoreCodeKey => "HS_CODE1";

        [SerializeField]
        private CanvasGroup topScoreCG;
        [SerializeField]
        private CanvasGroup newRecordCG;
        [SerializeField]
        private CanvasGroup continueNotice;
        [SerializeField]
        private Text currentScore;
        [SerializeField]
        private Text topScore;

        public void Hide()
        {
            transform.localScale = Vector3.zero;
        }
        
        private void Awake() => ResetState();

        public void ResetState()
        {
            continueNotice.DOKill(true);
            newRecordCG.DOKill(true);
            
            newRecordCG.alpha = 0;
            topScoreCG.alpha = 0;
            continueNotice.alpha = 0;
            transform.localScale = Vector3.zero;
        }

        public void Show(int playerScore)
        {
            transform.DOScale(Vector3.one, 0.3f)
                .OnComplete(() => CalculateScore(playerScore));
        }

        public void ShowContinueNotice()
        {
            continueNotice.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }

        private void CalculateScore(int playerScore)
        {
            var currentHighScore = playerScore;
            var lastHighScore = 0;
            if (PlayerPrefs.HasKey(HighScoreCodeKey))
                lastHighScore = PlayerPrefs.GetInt(HighScoreCodeKey);

            var isNewRecord = currentHighScore > lastHighScore;

            DOTween.To(() => 0, value => currentScore.text = $"Score : {value:00000000}", currentHighScore, 1f)
                .OnComplete(() =>
                {
                    topScore.text = $"Top Score : {(isNewRecord ? currentHighScore : lastHighScore):00000000}";
                    topScoreCG.DOFade(1, 0.2f);

                    if (isNewRecord)
                    {
                        newRecordCG.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
                        PlayerPrefs.SetInt(HighScoreCodeKey, currentHighScore);
                    }
                });
        }
    }
}