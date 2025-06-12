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

    // Note: Pas d'héritage pour les structs, donc plusieurs solutions : soit de la composition mais ça veut
    // dire qu'on ne peut pas stocker toutes les entités ensemble, j'ai donc préféré utiliser une union ici
    // (bien que ça n'existe pas en C#), où on a des données supplémentaires en fonction du type d'entité.
    [Serializable]
    public struct Entity {
        public Vec3F32 Position; // Mal nommé : devrait s'appeler `Center`.
        public Vec2I32[] Coords; // Coordonnées des cases occupées sur la grille 2D de la région.
        public int Id;
        public EntityType Type;
        public bool Destroy;
        
        public MermaidData MermaidData;
        public JellyfishData JellyfishData;
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

        public static readonly Dictionary<EntityType, int> EntityScoreValues = new () {
            { EntityType.Coin, 1 },
            { EntityType.Mermaid, 5 },
            { EntityType.Jellyfish, 5 },
        };

        public static readonly Dictionary<EntityType, int> EntitySpawnPcts = new () {
            { EntityType.Coin, 80 },
            { EntityType.Mermaid, 40 },
            { EntityType.Jellyfish, 40 },
        };
    }
    
    public static class EntityLogic {
        private static int _currentId;
        public static int NextId => ++_currentId;

        public static Entity CreateEntityFromCell(EntityCell entityCell, int offsetZ) {
            var e = new Entity();
            e.Id = NextId;
            e.Type = entityCell.Type;
            
            e.Coords = GetAllEntityCoords(entityCell, offsetZ);
            e.Position = GetPosition(e.Type, e.Coords);
            
            return e;
        }
        
        public static Vec2I32 GetEntityTypeDimension(EntityType entityType) {
            var result = Vec2I32.One;
            
            if (EntityConf.DimensionByEntityType.TryGetValue(entityType, out var value)) {
                result = value;
            }
            
            return result;
        }

        // Calcul de toutes les cases prises par une entité sur la grille 2D.
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
        
        // On fait un calcul pour avoir le centre d'une entité en fonction des cases qu'elle occupe.
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
