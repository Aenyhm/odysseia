using Sources;
using UnityEngine;

namespace Unity.Scripts {
    public class UnityPlatform : IPlatform {
        public string PersistentPath => Application.persistentDataPath;

        public void Log(string message) {
            Debug.Log(message);
        }
        
        public string Serialize(object obj) {
            return JsonUtility.ToJson(obj);
        }
        
        public T Deserialize<T>(string json) {
            return JsonUtility.FromJson<T>(json);
        }
    }
}
