using Newtonsoft.Json;
using UnityEngine;

namespace Save_Load
{
    public class PlayerPrefsSaveLoad : SaveLoad
    {
        private static PlayerPrefsSaveLoad _instance;
        public static PlayerPrefsSaveLoad Current => _instance ??= new PlayerPrefsSaveLoad();
        
        public override void Save<T>(T data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            
            PlayerPrefs.SetString(typeof(T).Name, jsonData);
            PlayerPrefs.Save();
        }

        public override bool Load<T>(out T data)
        {
            if (PlayerPrefs.HasKey(typeof(T).Name))
            {
                var json = PlayerPrefs.GetString(typeof(T).Name);

                data = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }
    }
}