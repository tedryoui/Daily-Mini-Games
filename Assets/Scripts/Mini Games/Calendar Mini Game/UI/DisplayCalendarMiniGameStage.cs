using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Login_Mini_Game.UI
{
    public class DisplayCalendarMiniGameStage : MonoBehaviour
    {
        [SerializeField] private int _stageIndex;

        [Header("References")] 
        [SerializeField] private TextMeshProUGUI _dayText;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _coins;
        [SerializeField] private GameObject _obtained;

        private CalendarMiniGame _miniGame;

        private void Awake()
        {
            _miniGame = GetComponentInParent<CalendarMiniGame>();
        }

        private void Update()
        {
            var stage = _miniGame.GetStage(_stageIndex);

            if (stage == null) return;
            
            UpdateVisuals(stage);
        }

        private void UpdateVisuals(Stage stage)
        {
            _dayText.SetText(stage.day.ToString());
            _obtained.SetActive(DailyHandler.Instance.Streak >= stage.day);
            _image.sprite = Player.Instance.GetCoinsIcon();
            _coins.SetText($"x{stage.coins.ToString()}");
        }
    }
}