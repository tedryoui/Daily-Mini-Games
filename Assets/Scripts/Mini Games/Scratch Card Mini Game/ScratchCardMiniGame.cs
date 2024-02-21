using System;
using System.Collections.Generic;
using System.Linq;
using __App.Scripts.Utilities.Scratch_Card;
using DefaultNamespace.Mini_Game_Base;
using Mini_Games.Scratch_Card_Mini_Game;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Scratch_Card_Mini_Game
{
    public class ScratchCardMiniGame : MiniGame
    {
        [Header("References")] 
        [SerializeField] private ScratchCardMiniGameReward _reward;
        [SerializeField] private List<ScratchCardDrawer> _drawers;

        [Header("Data presets")] 
        [SerializeField] private int _attempts;
        [SerializeField] private ScratchCard[] _cards;

        [Header("Callbacks")] 
        [Space(10)] public UnityEvent finish;
        
        private List<int> _startedCardIndices;
        private bool _isDrawersBlocked;

        private void Start()
        {
            ResetCards();
            PrepareDrawers();

            if (!IsAvailable) BlockDrawers();
        }

        private void PrepareDrawers()
        {
            foreach (var drawer in _drawers)
            {
                drawer.CardScratched.AddListener(ProcessDrawer);
            }
        }

        private void ProcessDrawer(ScratchCardDrawer card)
        {
            var index = _drawers.IndexOf(card);

            if (_startedCardIndices.Contains(index)) return;
            
            if (card.GetFillPercentage >= 0.1f) DisplayReward(index);

            if (_startedCardIndices.Count >= _attempts) Finish();
        }

        private void DisplayReward(int index)
        {
            _startedCardIndices.Add(index);
            _reward.ShowReward(Player.Instance.GetCoinsIcon(), _cards[index].coins);
            finish?.Invoke();
        }

        private void BlockDrawers()
        {
            foreach (var drawer in _drawers)
            {
                var index = _drawers.IndexOf(drawer);
                
                drawer.Block(!_startedCardIndices.Contains(index));
            }

            _isDrawersBlocked = true;
        }

        private void ResetCards()
        {
            foreach (var drawer in _drawers)
                drawer.ResetCard();

            _startedCardIndices = new List<int>();

            ShuffleDrawersAndCards();
        }

        private void ShuffleDrawersAndCards()
        {
            var rnd = new System.Random();
            var rndList = new List<int>();

            foreach (var card in _cards) rndList.Add(rnd.Next());

            var cardsCopy = (_cards.Clone() as ScratchCard[]).ToList();
            _cards = _cards.OrderBy(x => rndList[cardsCopy.IndexOf(x)]).ToArray();

            var drawersCopy = (_drawers.ToArray().Clone() as ScratchCardDrawer[]).ToList();
            _drawers = _drawers.OrderBy(x => rndList[drawersCopy.IndexOf(x)]).ToList();

            foreach (var drawer in _drawers) drawer.transform.SetSiblingIndex(_drawers.IndexOf(drawer));
        }

        private void Update()
        {
            if (_isDrawersBlocked && IsAvailable)
            {
                ResetCards();
                _isDrawersBlocked = false;
            }
        }

        public override void LoadDefault()
        {
            base.LoadDefault();
            
            ResetCards();
        }

        public override void Play()
        {
            
        }

        public override void Finish()
        {
            ObtainReward();
            BlockDrawers();
            TrackPlayed();
        }

        private void ObtainReward()
        {
            foreach (var index in _startedCardIndices)
            {
                var card = _cards[index];
                
                Player.Instance.AddCoins(card.coins);
            }
        }
    }
}