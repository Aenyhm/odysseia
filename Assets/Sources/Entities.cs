using System;
using Sources.Toolbox;

namespace Sources {
    
    public enum EntityType : byte {
        Boat,
        Rock,
        Trunk,
        Coin,
    }
    
    [Serializable]
    public struct Entity {
        public Vec3F32 Position;
        public Vec3F32 Size;
        public int Id;
    }
    
    public static class EntityManager {
        private static int _currentId;
        public static int NextId => ++_currentId;
    }
}
