using DefaultNamespace.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Spin_Mini_Game
{
    public class SpinMiniGameReward : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DOSmoothCanvasGroup _rewardSmooth;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _rewardText;

        [Header("Tween Settings")] 
        [SerializeField] private float _intervalDuration;

        [Header("Callbacks")] 
        [Space(10)] public UnityEvent showed;
        
        public Tween ShowReward(Sprite icon, string message)
        {
            _rewardSmooth.Disable();
            _rewardIcon.sprite = icon;
            _rewardText.SetText(message);
            
            var sequence = DOTween.Sequence();

            sequence.Append(_rewardSmooth.EnableSmooth(0));
            sequence.Append(DOVirtual.DelayedCall(0.0f, () => showed?.Invoke()));
            sequence.AppendInterval(_intervalDuration);
            sequence.Insert(2.0f, _rewardSmooth.DisableSmooth(0));

            return sequence;
        }
    }
}