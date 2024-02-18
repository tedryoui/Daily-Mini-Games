using System;
using Save_Load;
using UnityEngine;

namespace DefaultNamespace
{
    public class Player : MonoBehaviour
    {
        #region Singleton

        private static Player _instance;

        public static Player Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<Player>();
                }

                return _instance;
            }
        }

        private void SingletonAwake()
        {
            _instance = this;
        }

        #endregion

        [SerializeField] private int _coins;
        [SerializeField] private Sprite _coinsIcon;
        
        private void Awake()
        {
            SingletonAwake();

            _coins = 0;
        }

        public int GetCoins()
        {
            return _coins;
        }

        public void AddCoins(int value)
        {
            _coins += value;
        }
        
        public Sprite GetCoinsIcon()
        {
            return _coinsIcon;
        }
    }
}