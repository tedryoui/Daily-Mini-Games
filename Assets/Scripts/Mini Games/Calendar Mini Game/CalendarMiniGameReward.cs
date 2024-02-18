using DefaultNamespace.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Login_Mini_Game
{
    public class CalendarMiniGameReward : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DOSmoothCanvasGroup _rewardSmooth;
        [SerializeField] private TextMeshProUGUI _rewardDayText;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _rewardText;

        [Header("Tween Settings")] 
        [SerializeField] private float _intervalDuration;

        public Tween ShowReward(int day, Sprite icon, string message)
        {
            _rewardSmooth.Disable();
            
            _rewardDayText.SetText($"Day {day}");
            _rewardIcon.sprite = icon;
            _rewardText.SetText(message);
            
            var sequence = DOTween.Sequence();

            sequence.Append(_rewardSmooth.EnableSmooth(0));
            sequence.AppendInterval(_intervalDuration);
            sequence.Insert(2.0f, _rewardSmooth.DisableSmooth(0));

            return sequence;
        }
    }
}