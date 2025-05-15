using System;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts {
    [Serializable]
    public struct GameObjectByType {
        public EntityType entityType;
        public GameObject gameObject;
    }
    
    [CreateAssetMenu(fileName = "Configuration", menuName = "Scriptable Objects/Configuration")]
    public class ConfigurationScriptable : ScriptableObject {
        public GameObjectByType[] CollidingObjects;
    }
}
