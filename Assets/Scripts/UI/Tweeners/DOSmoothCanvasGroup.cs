using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DOSmoothCanvasGroup : MonoBehaviour
    {
        [Header("In")]
        [SerializeField] private Ease _easeIn;
        [SerializeField] private float _durationIn;
        
        [Header("Out")]
        [SerializeField] private Ease _easeOut;
        [SerializeField] private float _durationOut;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void EnableSmooth_UI(float delay)
        {
            // Fade canvas group
            _canvasGroup.DOFade(1.0f, _durationIn).OnComplete(Enable)
                .SetEase(_easeIn)
                .SetDelay(delay)
                .OnStart(Disable);
        }

        public Tween EnableSmooth(float delay)
        {
            // Fade canvas group
            return _canvasGroup.DOFade(1.0f, _durationIn).OnComplete(Enable)
                .SetEase(_easeIn)
                .SetDelay(delay)
                .OnStart(Disable);
        }
        
        public void DisableSmooth_UI(float delay)
        {
            // Fade canvas group
            _canvasGroup.DOFade(0.0f, _durationOut).OnComplete(Disable)
                .SetEase(_easeOut)
                .SetDelay(delay)
                .OnStart(Enable);
        }
        
        public Tween DisableSmooth(float delay)
        {
            // Fade canvas group
            return _canvasGroup.DOFade(0.0f, _durationOut).OnComplete(Disable)
                .SetEase(_easeOut)
                .SetDelay(delay)
                .OnStart(Enable);
        }

        public void Enable()
        {
            // Enable canvas group interaction
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1.0f;
        }

        public void Disable()
        {
            // Disable canvas group interaction
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.0f;
        }
    }
}