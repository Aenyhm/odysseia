using Sources;
using UnityEngine;

namespace Unity.Scripts {
    public class UnityPlatform : IPlatform {
        public string PersistentPath => Application.persistentDataPath;

        public void Log(string message) {
            #if UNITY_EDITOR
            Debug.Log(message);
            #endif
        }
        
        public void LogWarn(string message) {
            #if UNITY_EDITOR
            Debug.LogWarning(message);
            #endif
        }
        
        public string Serialize(object obj) {
            return JsonUtility.ToJson(obj);
        }
        
        public T Deserialize<T>(string json) {
            return JsonUtility.FromJson<T>(json);
        }
    }
}
