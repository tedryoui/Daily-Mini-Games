using DefaultNamespace.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mini_Games.Scratch_Card_Mini_Game
{
    public class ScratchCardMiniGameReward : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DOSmoothCanvasGroup _canvasGroup;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Tween Settings")] 
        [SerializeField] private float _punchStrength;
        [SerializeField] private float _duration;
        
        private readonly string _showRewardTweenDefaultId = "ShowReward-Tween";
        
        public Tween ShowReward(Sprite icon, int amount)
        {
            _image.sprite = icon;
            _text.SetText(amount.ToString());

            DOTween.Kill(_showRewardTweenDefaultId, true);
            
            var sequence = DOTween.Sequence();
            sequence.SetId(_showRewardTweenDefaultId);

            sequence.Append(_canvasGroup.EnableSmooth(0));
            sequence.Insert(0.0f, transform.DOPunchScale(Vector3.one * _punchStrength, _duration, 2));
            sequence.AppendInterval(0.25f);
            sequence.Append(_canvasGroup.DisableSmooth(0));
            
            return sequence;
        }
    }
}