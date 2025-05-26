using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    
    public enum EntityType : byte {
        None,
        Boat,
        Rock,
        Trunk,
        Coin,
        Mermaid,
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

    public static class EntityDefinitions {
        public static readonly Dictionary<EntityType, Vec2I32> DimensionByEntityType = new() {
            { EntityType.None,    new Vec2I32(1, 1) },
            { EntityType.Rock,    new Vec2I32(1, 1) },
            { EntityType.Trunk,   new Vec2I32(2, 1) },
            { EntityType.Coin,    new Vec2I32(1, 2) },
            { EntityType.Mermaid, new Vec2I32(1, 1) },
        };

        private static readonly Dictionary<RegionType, EntityType[]> EntitiesByRegion = new() {
            { RegionType.Aegis, new[] { EntityType.Rock, EntityType.Trunk, EntityType.Mermaid } },
            { RegionType.Styx, new EntityType[] {} },
            { RegionType.Olympia, new EntityType[] {} },
            { RegionType.Hephaestus, new EntityType[] {} },
            { RegionType.Artemis, new EntityType[] {} },
        };
            
        public static bool IsEntityTypeAvailableForRegion(EntityType entityType, RegionType regionType) {
            if (entityType == EntityType.Coin) return true;
            
            foreach (var type in EntitiesByRegion[regionType]) {
                if (type == entityType) return true;
            }
            
            return false;
        }
    }
}
