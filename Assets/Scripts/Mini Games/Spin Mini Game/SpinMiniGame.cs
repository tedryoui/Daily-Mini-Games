using DefaultNamespace.Mini_Game_Base;
using DefaultNamespace.Scratch_Card_Mini_Game;
using DefaultNamespace.UI;
using DG.Tweening;
using Spin_Mini_Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.Spin_Mini_Game
{
    public class SpinMiniGame : MiniGame
    {
        [Header("References")]
        [SerializeField] private RectTransform _spinner;
        [SerializeField] private SpinMiniGameReward _reward;
        
        [Header("Spin Forward settings")]
        [SerializeField] private Vector2Int _spinRoundRotationsRange;
        [SerializeField] private float _spinDuration;
        [SerializeField] private Ease _spinEase;

        [Header("Spin Backward settings")]
        [SerializeField] private float _spinBackRotation;
        [SerializeField] private float _spinBackDelay;
        
        [Header("Callbacks")]
        public UnityEvent play;
        [Space(10)] public UnityEvent finish;

        [Header("Data presets")]
        [SerializeField] private Station[] _stations;
        
        private readonly string _spinTweenDefaultId = "Spin-Tween";
        private int _selectedStationIndex;

        private void OnEnable()
        {
            // Kill spin tween, if it already exists
            DOTween.Kill(_spinTweenDefaultId, true);
        }

        public override void Play()
        {
            if (!IsAvailable) return;
            
            // Kill spin tween, if it already exists
            DOTween.Kill(_spinTweenDefaultId, true);
         
            play?.Invoke();
            
            // Choose station
            _selectedStationIndex = UnityEngine.Random.Range(0, _stations.Length);
            
            // Build spin tween and track it with default id
            var spin = Spin();
            spin.SetId("Spin-Tween");

            // Set spin callbacks
            spin.OnStart(Begin);
            spin.OnComplete(Finish);

            // Start spin tween
            spin.Play();
        }

        private void Begin()
        {
            
        }

        public override void Finish()
        {
            ObtainReward();
            
            // Call finish event callbacks
            finish?.Invoke();

            _selectedStationIndex = 0;
            
            // Update Daily Mini Game state
            TrackPlayed();
        }

        private void ObtainReward()
        {
            var station = _stations[_selectedStationIndex];
            
            Player.Instance.AddCoins(station.coins);
        }

        #region Spin Animations

        public Sequence Spin()
        {
            // Create new sequence
            var sequence = DOTween.Sequence();

            // Compute round rotations
            var roundSpinsCount = UnityEngine.Random.Range(_spinRoundRotationsRange.x, _spinRoundRotationsRange.y);
            var roundSpinAngle = roundSpinsCount * 360.0f;

            // Get target rotation from selected station
            var stationRotationAngle = _stations[_selectedStationIndex].rotation;

            var overallRotation = roundSpinAngle + stationRotationAngle;

            // Append rotation sequence elements
            
            // Rollback spinner to 0.0f z rotation
            if (_spinner.localRotation.z != 0)
            {
                sequence.Append(DOVirtual.Float(_spinner.localRotation.eulerAngles.z, 0.0f, _spinBackRotation,
                    AdjustSpinnerRotation));
                sequence.AppendInterval(_spinBackDelay);
            }
            
            sequence.Append(DOVirtual.Float(0.0f, overallRotation, _spinDuration, AdjustSpinnerRotation)
                .SetEase(_spinEase));
            sequence.Append(FinishSpinAnimation());

            return sequence;
        }
        
        private Sequence FinishSpinAnimation()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(_spinner.DOPunchRotation(new Vector3(0.0f, 0.0f, 1.0f), 0.5f, 10, 4));
            sequence.Append(_reward.ShowReward(Player.Instance.GetCoinsIcon(), _stations[_selectedStationIndex].coins.ToString()));
            
            return sequence;
        }

        private void AdjustSpinnerRotation(float rotation)
        {
            _spinner.localRotation = Quaternion.Euler(0.0f, 0.0f, rotation);
        }
        
        #endregion
    }
}