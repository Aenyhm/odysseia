using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    public enum RegionType {
        Aegis,
        Styx,
        Olympia,
        Hephaestus,
        Artemis
    }
    
    public struct Obstacle {
        public Vec3F32 Position;
        public Vec3F32 Size;
        public EntityType Type;
        public int Id;
    }
    
    public struct Region {
        public RegionType Type;
        public List<Obstacle> Obstacles;
    }

    public static class RegionSystem {
        public const int LANE_DISTANCE = 4;
        
        private const int OBSTACLE_SPAWN_DISTANCE = 20;
        private const int OBSTACLE_SPAWN_Z = 150;
        private const int OBSTACLE_REMOVE_DISTANCE = 20;

        private static float _lastObstacleSpawnZ;
        private static Conf _conf;
        
        public static void Init(in Conf conf, out Region region) {
            _conf = conf;
            region = new Region();
            region.Type = (RegionType)Services.Get<Random>().Next(5); //RegionType.Aegis;
            region.Obstacles = new List<Obstacle>();
        }
        
        public static void Update(ref Region region, float boatDistance) {
            if (boatDistance - _lastObstacleSpawnZ > OBSTACLE_SPAWN_DISTANCE) {
                var obstacle = GenerateObstable(boatDistance + OBSTACLE_SPAWN_Z);
                region.Obstacles.Add(obstacle);
                
                _lastObstacleSpawnZ = boatDistance;
            }

            region.Obstacles.RemoveAll(o => o.Position.Z < boatDistance - OBSTACLE_REMOVE_DISTANCE);
        }
        
        private static Obstacle GenerateObstable(float atZ) {
            var result = new Obstacle();
            result.Id = EntityManager.NextId;
            
            var lane = Services.Get<Random>().Next(-1, 2);
            result.Position = new Vec3F32(lane*LANE_DISTANCE, 0, atZ);
            
            result.Type = Services.Get<Random>().Next(2) == 0 ? EntityType.Rock : EntityType.Trunk;
            result.Size = _conf.Sizes[result.Type];

            return result;
        }
    }
}
