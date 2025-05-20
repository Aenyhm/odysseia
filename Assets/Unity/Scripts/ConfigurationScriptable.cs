using System;
using Sources;
using UnityEngine;

namespace Unity.Scripts {
    [Serializable]
    public struct GameObjectByType {
        public EntityType entityType;
        public GameObject gameObject;
    }

    [CreateAssetMenu(fileName = "Configuration", menuName = "Scriptable Objects/Configuration")]
    public class ConfigurationScriptable : ScriptableObject {
        
        [Tooltip("To let the game know the size of models")]
        public GameObjectByType[] CollidingObjects;
    }
}
