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
        Relic,
    }

    [Serializable]
    public struct Entity {
        public Vec3F32 Position;
        public Vec2I32[] Coords;
        public int Id;
        public EntityType Type;
        public bool Destroy;
        
        public MermaidData MermaidData;
    }

    public static class EntityConf {
        public static readonly Dictionary<EntityType, Vec2I32> DimensionByEntityType = new() {
            { EntityType.Trunk, new Vec2I32(2, 1) },
            { EntityType.Coin,  new Vec2I32(1, 2) },
        };

        public static readonly Dictionary<RegionType, EntityType[]> EntitiesByRegion = new() {
            { RegionType.Aegis, new[] { EntityType.Rock, EntityType.Trunk, EntityType.Mermaid, EntityType.Jellyfish } },
            { RegionType.Styx, new EntityType[] {} },
            { RegionType.Olympia, new EntityType[] {} },
            { RegionType.Hephaestus, new EntityType[] {} },
            { RegionType.Artemis, new EntityType[] {} },
        };
        
        public static readonly HashSet<EntityType> ObstacleTypes = new() {
            EntityType.Rock,
            EntityType.Trunk,
            EntityType.Mermaid,
            EntityType.Jellyfish,
        };
        
        public static readonly HashSet<EntityType> DestroyableEntityTypes = new() {
            EntityType.Trunk,
            EntityType.Mermaid,
            EntityType.Jellyfish,
        };
    }
    
    public static class EntityLogic {
        private static int _currentId;
        public static int NextId => ++_currentId;
        
                
        public static Vec2I32 GetEntityTypeDimension(EntityType entityType) {
            var result = Vec2I32.One;
            
            if (EntityConf.DimensionByEntityType.TryGetValue(entityType, out var value)) {
                result = value;
            }
            
            return result;
        }

        public static Vec2I32[] GetAllEntityCoords(EntityCell cell, int offsetZ) {
            var dimensions = GetEntityTypeDimension(cell.Type);
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
            var dimensions = GetEntityTypeDimension(entityType);

            // 1 => (1 - 1)/2 = 0
            // 2 => (2 - 1)/2 = 0.5
            // 3 => (3 - 1)/2 = 1
            var offset = (dimensions - Vec2F32.One)/2f;
            
            var posX = LaneLogic.GetPosition(coords[0].X + offset.X);
            var posZ = (coords[0].Y + offset.Y)*CoreConfig.GridScale;
            
            return new Vec3F32(posX, 0, posZ);
        }

        public static bool IsEntityTypeAvailableForRegion(EntityType entityType, RegionType regionType) {
            if (entityType == EntityType.Coin) return true;
            
            foreach (var type in EntityConf.EntitiesByRegion[regionType]) {
                if (type == entityType) return true;
            }
            
            return false;
        }
    }
}
