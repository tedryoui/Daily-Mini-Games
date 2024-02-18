using System;
using System.Globalization;
using Newtonsoft.Json;
using Save_Load;
using UnityEngine;

namespace DefaultNamespace
{
    public class DailyHandler : MonoBehaviour, ISavable
    {
        #region Singleton

        private static DailyHandler _instance;

        public static DailyHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<DailyHandler>();
                }

                return _instance;
            }
        }

        private void SingletonAwake()
        {
            _instance = this;
        }

        #endregion
        
        private int _streak;
        private DateTime _loginDate;

        public int Streak => _streak;

        public bool CanUpdate => DateTime.Compare(DateTime.Now, _loginDate.AddDays(1)) >= 0;
        
        private void UpdateStreak()
        {
            _streak++;
            _loginDate = DateTime.Today;
        }

        #region Unity Events

        private void FixedUpdate()
        {
            if (CanUpdate) UpdateStreak();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Save();
        }
        
        private void Awake()
        {
            SingletonAwake();
            
            Load();
        }

        #endregion

        #region Save & Load
        
        [Serializable]
        public class DailyHandlerSaveData : AbstractSaveData<DailyHandler>
        {
            public string loginDateStamp;
            public int streak;

            public DailyHandlerSaveData()
            {
                
            }
            
            public DailyHandlerSaveData(DailyHandler handler)
            {
                loginDateStamp = handler._loginDate.ToString(CultureInfo.InvariantCulture);
                streak = handler._streak;
            }

            public override void Insert(DailyHandler handler)
            {
                handler._loginDate = DateTime.Parse(loginDateStamp, CultureInfo.InvariantCulture);
                handler._streak = streak;
            }
        }
        
        public void Save()
        {
            var data = new DailyHandlerSaveData(this);
            
            PlayerPrefsSaveLoad.Current.Save(data);
        }

        public void Load()
        {
            var hasData = PlayerPrefsSaveLoad.Current.Load(out DailyHandlerSaveData data);
            
            if (!hasData)
                LoadDefault();
            else
                data.Insert(this);
        }

        public void LoadDefault()
        {
            _loginDate = DateTime.Today;
            _streak = 1;
        }

        #endregion
    }
}