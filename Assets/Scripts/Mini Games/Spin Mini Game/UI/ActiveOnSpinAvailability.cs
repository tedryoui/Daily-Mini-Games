using System;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Spin_Mini_Game
{
    public class ActiveOnSpinAvailability : MonoBehaviour
    {
        [SerializeField] private SpinMiniGame _spinMiniGame;

        [Space(10)]
        public UnityEvent<bool> callback;

        private void Update()
        {
            callback?.Invoke(_spinMiniGame.IsAvailable);
        }
    }
}