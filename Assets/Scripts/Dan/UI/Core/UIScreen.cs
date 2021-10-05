using System;
using DG.Tweening;
using UnityEngine;

namespace Dan.UI.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField]
        private bool hideOnStart;
        
        private CanvasGroup _canvasGroup;

        public void Show(float fadeTime = 0, Action actionOnShow = null)
        {
            _canvasGroup.DOFade(1, fadeTime)
                .OnComplete(() => actionOnShow?.Invoke());
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
        
        public void Hide(float fadeTime = 0, Action actionOnHide = null)
        {
            _canvasGroup.DOFade(0, fadeTime)
                .OnComplete(() => actionOnHide?.Invoke());
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
        

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (hideOnStart)
                Hide();
        }
    }
}