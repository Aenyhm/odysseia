using System;
using Sources;
using UnityEngine;

namespace Unity.Scripts.Scriptables {
    
    [Serializable]
    public struct GameObjectByType {
        public EntityType entityType;
        public GameObject gameObject;
    }

    [CreateAssetMenu(fileName = "Colliding Objects", menuName = "Scriptable Objects/Colliding Objects")]
    public class CollidingScriptable : ScriptableObject {
        
        [Tooltip("To let the game know the size of colliding boxes")]
        public GameObjectByType[] CollidingObjects;
    }
}
