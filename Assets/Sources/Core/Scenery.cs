using System;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public class Obstacle : Entity {
    }
    
    public class Scenery {
        public readonly Pool<Obstacle> obstacles = new(() => new Obstacle());
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
            
            var releasedObstacles = scenery.obstacles.FreeAll(obstacle =>
                obstacle.transform.position.z < boat.transform.position.z - OBSTACLE_REMOVE_DISTANCE
            );
            
            foreach (var obstacle in releasedObstacles) {
                Services.Get<Platform>().renderer.Destroy(obstacle);
            }
        }
        
        private static void GenerateObstable(Scenery scenery, float atZ) {
            var obstacle = scenery.obstacles.Get();
            EntityManager.Reset(obstacle);
            
            if (_rnd.Next(2) == 0) {
                SetRock(obstacle);
            } else {
                SetTrunk(obstacle);
            }

            var lane = _rnd.Next(-1, 2);
            obstacle.transform.position = new Vec3F32(lane*LANE_DISTANCE, 0, atZ);

            Services.Get<Platform>().renderer.Create(obstacle);
         }
         
        private static void SetRock(Obstacle obstacle) {
            obstacle.transform.size = new Vec3F32(2f, 1.8f, 2f);
            obstacle.transform.position = new Vec3F32(10, 0, 70);
            obstacle.color = new Vec3F32(0.4f, 0.4f, 0.4f);
        }

        private static void SetTrunk(Obstacle obstacle) {
            obstacle.transform.position = new Vec3F32(-10, 0, 40);
            obstacle.transform.size = new Vec3F32(4, 1, 1);
            obstacle.color = new Vec3F32(0.55f, 0.27f, 0.075f);
        }
    }
}
