using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class DisplayDailyStreak : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Update()
        {
            _text.SetText(DailyHandler.Instance.Streak.ToString());
        }
    }
}