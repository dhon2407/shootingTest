using System.Collections;
using Dan.Manager;
using Dan.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI.HUD
{
    public class PauseScreen : UIScreen
    {
        [SerializeField]
        private Text message;
        [SerializeField]
        private ScoreSummary scoreSummary;

        public void Pause(bool isPause)
        {
            scoreSummary.Hide();
            
            message.text = "Game Pause";
            
            if (isPause)
                Show();
            else
                Hide();
            
            Time.timeScale = isPause ? 0 : 1;
        }

        public void EndGame(int playerScore)
        {
            scoreSummary.ResetState();
            message.text = "Game over";
            Show(0.2f, () =>
            {
                scoreSummary.Show(playerScore);
                StartCoroutine(WaitForRestartInput());
            });
            
        }

        private IEnumerator WaitForRestartInput()
        {
            yield return new WaitForSeconds(1f);
            
            scoreSummary.ShowContinueNotice();
            
            while (!Input.anyKeyDown)
                yield return null;

            GameFlowManager.ReturnToTitle();
        }

        protected override void Awake()
        {
            base.Awake();
            scoreSummary.Hide();
        }
    }
}