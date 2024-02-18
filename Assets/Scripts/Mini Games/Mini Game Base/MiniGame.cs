using System;
using Newtonsoft.Json;
using Save_Load;
using UnityEngine;

namespace DefaultNamespace.Mini_Game_Base
{
    public abstract class MiniGame : MonoBehaviour, ISavable
    {
        private int _streakStamp;

        public bool IsAvailable => _streakStamp != DailyHandler.Instance.Streak;

        public abstract void Play();
        public abstract void Finish();

        protected void TrackPlayed()
        {
            _streakStamp = DailyHandler.Instance.Streak;
        }
        
        #region Unity Events

        protected virtual void Awake()
        {
            Load();
        }

        protected virtual void OnApplicationQuit()
        {
            Save();
        }

        protected virtual void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) Save();
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Save();
        }

        #endregion


        #region Save & Load

        [Serializable]
        public class MiniGameSaveData : AbstractSaveData<MiniGame>
        {
            public int streakStamp;

            public MiniGameSaveData()
            {
                
            }

            public MiniGameSaveData(MiniGame handler)
            {
                streakStamp = handler._streakStamp;
            }

            public override void Insert(MiniGame handler)
            {
                handler._streakStamp = streakStamp;
            }
        }
        
        public virtual void Save()
        {
            var saveData = new MiniGameSaveData(this);
            
            PlayerPrefsSaveLoad.Current.Save(saveData);
        }

        public virtual void Load()
        {
            var hasData = PlayerPrefsSaveLoad.Current.Load<MiniGameSaveData>(out var data);
            
            if(hasData)
                data.Insert(this);
            else 
                LoadDefault();
        }

        public virtual void LoadDefault()
        {
            _streakStamp = -1;
        }

        #endregion
    }
}