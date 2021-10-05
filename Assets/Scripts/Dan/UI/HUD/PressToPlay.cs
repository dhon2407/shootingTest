using DG.Tweening;
using UnityEngine;

namespace Dan.UI.HUD
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PressToPlay : MonoBehaviour
    {
        [SerializeField]
        private float flickerDuration = 1f;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start() => StartAnimating();

        private void StartAnimating()
        {
            _canvasGroup.DOFade(0, flickerDuration)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}