using System.Linq;
using DefaultNamespace.Mini_Game_Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Login_Mini_Game
{
    public class CalendarMiniGame : MiniGame
    {
        [Header("References")] 
        [SerializeField] private CalendarMiniGameReward _reward;

        [Header("Data presets")] 
        [SerializeField] private Stage[] _stages;
        private Stage _selectedStage;

        [Header("Callback")] 
        [Space(10)] public UnityEvent finish;
        
        private readonly string _calendarTweenDefaultId = "Calendar-Tween";
        
        private void OnEnable()
        {
            // Kill spin tween, if it already exists
            DOTween.Kill(_calendarTweenDefaultId, true);
        }
        
        private void FixedUpdate()
        {
            if (IsAvailable) Play();
        }

        public override void Play()
        {
            TrackPlayed();
            
            // Choose stage
            var day = DailyHandler.Instance.Streak;
            _selectedStage = _stages.FirstOrDefault(x => x.day.Equals(day));
            
            if (_selectedStage != null)
                Finish();
        }

        public override void Finish()
        {
            ObtainReward();
            AnimateReward();
            
            finish?.Invoke();
        }

        private void ObtainReward()
        {
            Player.Instance.AddCoins(_selectedStage.coins);
        }

        private void AnimateReward()
        {
            if (_selectedStage != null)
            {
                // Kill spin tween, if it already exists
                DOTween.Kill(_calendarTweenDefaultId, true);
                
                // Build spin tween and track it with default id
                var sequence = DOTween.Sequence();
                sequence.SetId(_calendarTweenDefaultId);

                sequence.Append(_reward.ShowReward(_selectedStage.day, Player.Instance.GetCoinsIcon(), $"x{_selectedStage.coins}"));

                sequence.Play();
            }
        }

        public Stage GetStage(int stageIndex)
        {
            if (stageIndex >= 0 && stageIndex < _stages.Length)
                return _stages[stageIndex];

            return null;
        }
    }
}