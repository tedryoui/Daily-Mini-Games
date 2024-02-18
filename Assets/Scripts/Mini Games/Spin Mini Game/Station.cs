using System;
using UnityEngine;

namespace DefaultNamespace.Scratch_Card_Mini_Game
{
    [Serializable]
    public class Station
    {
        public float rotation;

        [Header("Reward")] 
        public int coins;
    }
}