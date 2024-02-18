using System;
using UnityEngine;

namespace DefaultNamespace.Login_Mini_Game
{
    [Serializable]
    public class Stage
    {
        public int day;

        [Header("Reward")] 
        public int coins;
    }
}