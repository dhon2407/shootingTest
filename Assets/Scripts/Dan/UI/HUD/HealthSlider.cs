using System;
using Dan.Manager;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dan.UI.HUD
{
    public class HealthSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private Gradient colorLevels;
        [SerializeField]
        private Image fill;

        public void SetValue(float newLevel)
        {
            AnimateFill(newLevel, 0.2f);
        }

        private void Awake()
        {
            GameFlowManager.OnGameStart += GameStart;
        }

        private void OnDestroy()
        {
            GameFlowManager.OnGameStart -= GameStart;
        }

        private void GameStart()
        {
            AnimateFill(1,0.5f);
        }
        
        private void AnimateFill(float newLevel, float speed)
        {
            DOTween.To(() => slider.value, value =>
            {
                slider.value = value;
                fill.color = colorLevels.Evaluate(value);
            }, newLevel, speed);
        }
    }
}