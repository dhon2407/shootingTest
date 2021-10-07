using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI.HUD
{
    public class LevelIndicator : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audio;
        
        private Text _text;

        public void Show(int level)
        {
            _text.text = $"Level {level}";
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    transform.DOScale(Vector3.zero, 0.2f).SetDelay(1f);
                });
            
            if (level > 1)
                audio.PlayOneShot(audio.clip);
        }

        private void Awake()
        {
            _text = GetComponent<Text>();
            transform.localScale =Vector3.zero;
        }
    }
}