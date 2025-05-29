using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    
    // Ne pas changer l'ordre car des données sont déjà sérialisées avec ces valeurs.
    public enum EntityType : byte {
        None,
        Boat,
        Rock,
        Trunk,
        Coin,
        Mermaid,
        Jellyfish,
        Cannonball,
    }

    [Serializable]
    public struct Entity {
        public Vec2I32[] Coords;
        public int Id;
        public EntityType Type;
    }

    public static class EntityConf {
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
        
        public static readonly HashSet<EntityType> DestroyableEntityTypes = new() {
            EntityType.Trunk,
            EntityType.Mermaid,
            EntityType.Jellyfish,
        };
            
        public static bool IsEntityTypeAvailableForRegion(EntityType entityType, RegionType regionType) {
            if (entityType == EntityType.Coin) return true;
            
            foreach (var type in EntitiesByRegion[regionType]) {
                if (type == entityType) return true;
            }
            
            return false;
        }
    }
    
    public static class EntityLogic {
        private static int _currentId;
        public static int NextId => ++_currentId;
        
        public static Vec2I32[] GetAllEntityCoords(EntityCell cell, int offsetZ) {
            var dimensions = EntityConf.DimensionByEntityType[cell.Type];
            var coords = new Vec2I32[dimensions.X*dimensions.Y];
            
            var i = 0;
            for (var y = 0; y < dimensions.Y; y++) {
                for (var x = 0; x < dimensions.X; x++) {
                    coords[i++] = new Vec2I32(cell.X + x, offsetZ + cell.Y + y);
                }
            }
            
            return coords;
        }
        
        public static Vec3F32 GetPosition(EntityType entityType, Vec2I32[] coords) {
            var dimensions = EntityConf.DimensionByEntityType[entityType];

            // 1 => (1 - 1)/2 = 0
            // 2 => (2 - 1)/2 = 0.5
            // 3 => (3 - 1)/2 = 1
            var offset = (dimensions - Vec2F32.One)/2f;
            
            var posX = LaneLogic.GetPosition(coords[0].X + offset.X);
            var posZ = (coords[0].Y + offset.Y)*CoreConfig.GridScale;
            
            return new Vec3F32(posX, 0, posZ);
        }
    }
}
