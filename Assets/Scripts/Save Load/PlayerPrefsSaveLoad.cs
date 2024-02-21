using Newtonsoft.Json;
using UnityEngine;

namespace Save_Load
{
    public class PlayerPrefsSaveLoad : SaveLoad
    {
        private static PlayerPrefsSaveLoad _instance;
        public static PlayerPrefsSaveLoad Current => _instance ??= new PlayerPrefsSaveLoad();
        
        public override void Save<T>(T data, string name = "")
        {
            name = (string.IsNullOrEmpty(name) ? typeof(T).Name : name);
            var jsonData = JsonConvert.SerializeObject(data);
            
            PlayerPrefs.SetString(name, jsonData);
            PlayerPrefs.Save();
        }

        public override bool Load<T>(out T data, string name = "")
        {
            name = (string.IsNullOrEmpty(name) ? typeof(T).Name : name);
            
            if (PlayerPrefs.HasKey(name))
            {
                var json = PlayerPrefs.GetString(name);

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