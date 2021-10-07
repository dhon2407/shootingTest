using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI.HUD
{
    public class LevelIndicator : MonoBehaviour
    {
        private Text _text;

        public void Show(string levelName)
        {
            _text.text = $"{levelName}";
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    transform.DOScale(Vector3.zero, 0.2f).SetDelay(1f);
                });
        }

        private void Awake()
        {
            _text = GetComponent<Text>();
            transform.localScale =Vector3.zero;
        }
    }
}