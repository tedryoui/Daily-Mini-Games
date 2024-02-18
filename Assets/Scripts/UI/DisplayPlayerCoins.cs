using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class DisplayPlayerCoins : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            _image.sprite = Player.Instance.GetCoinsIcon();
        }

        private void Update()
        {
            _text.SetText(Player.Instance.GetCoins().ToString());
        }
    }
}