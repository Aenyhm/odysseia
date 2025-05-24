using System;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    
    public enum EntityType : byte {
        Boat,
        Rock,
        Trunk,
        Coin,
        Mermaid,
        Jellyfish,
    }
    
    [Serializable]
    public struct Entity {
        public Vec3F32 Size;
        public Vec3F32 Position;
        public int Id;
        public EntityType Type;
        public LaneType LaneType;
    }
    
    public static class EntityManager {
        private static int _currentId;
        public static int NextId => ++_currentId;
    }
}
