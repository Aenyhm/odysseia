using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    
    public abstract class Obstacle : Entity { }
    public class Rock : Obstacle { }
    public class Trunk : Obstacle { }
    
    public class Scenery {
        public readonly Dictionary<Type, Pool<Obstacle>> obstaclePoolsByType = new() {
            { typeof(Rock),  new Pool<Obstacle>(() => new Rock())},
            { typeof(Trunk), new Pool<Obstacle>(() => new Trunk())},
        };
    }
    
    public static class SceneryController {
        public const int LANE_DISTANCE = 4;
        
        private const int OBSTACLE_SPAWN_DISTANCE = 10; //20;
        private const int OBSTACLE_SPAWN_Z = 80;
        private const int OBSTACLE_REMOVE_DISTANCE = 20;

        private static readonly Random _rnd = new();
        private static float _lastObstacleSpawnZ;
        
        public static void Update() {
            var gs = Services.Get<GameState>();
            var scenery = gs.scenery;

            var boat = gs.boat;
            
            if (boat.transform.position.z - _lastObstacleSpawnZ > OBSTACLE_SPAWN_DISTANCE) {
                GenerateObstable(scenery, boat.transform.position.z + OBSTACLE_SPAWN_Z);
                _lastObstacleSpawnZ = boat.transform.position.z;
            }
            
            var releasedObstacles = new List<Obstacle>();
            foreach (var obstacles in scenery.obstaclePoolsByType.Values) {
                releasedObstacles.AddRange(obstacles.FreeAll(obstacle =>
                    obstacle.transform.position.z < boat.transform.position.z - OBSTACLE_REMOVE_DISTANCE
                ));
            }
            
            foreach (var obstacle in releasedObstacles) {
                Services.Get<IPlatform>().RemoveEntityView(obstacle);
            }
        }
        
        private static void GenerateObstable(Scenery scenery, float atZ) {
            var obstacleType = _rnd.Next(2) == 0 ? typeof(Rock) : typeof(Trunk);
            
            var obstacle = scenery.obstaclePoolsByType[obstacleType].Get();
            EntityManager.Reset(obstacle);

            var lane = _rnd.Next(-1, 2);
            obstacle.transform.position = new Vec3F32(lane*LANE_DISTANCE, 0, atZ);

            Services.Get<IPlatform>().AddEntityView(obstacle);
        }
    }
}
