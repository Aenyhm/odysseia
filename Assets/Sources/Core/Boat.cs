using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public class Boat : Entity {
        public Vec3F32 velocity;
        public float speed;
        public int lane;
        public int health;
    }

    public static class BoatController {
        public const int HEALTH_MAX = 10;
        private const float SPEED_MIN = 1f;
        private const float SPEED_MAX = 30f;
        
        private static readonly HashSet<int> _collisionIds = new();

        public static Boat Create() {
            var boat = EntityManager.Create<Boat>();
            boat.speed = SPEED_MAX;
            boat.transform.position.y = 1f;
            boat.velocity.z = boat.speed;
            boat.health = HEALTH_MAX;
            
            Services.Get<IPlatform>().AddEntityView(boat);
            
            return boat;
        }

        public static void Update(float dt, GameInput input) {
            var gs = Services.Get<GameState>();

            var boat = gs.boat;

			CheckCollisions(boat, gs.scenery.activeObstacles);
            
            CheckHorizontalMove(boat, Convert.ToInt32(input.HorizontalAxis));

            boat.transform.position += boat.velocity*dt;
        }

		private static void CheckCollisions(Boat boat, List<Entity> obstacles) {
            foreach (var obstacle in obstacles) {
                if (Collisions.CheckAabb(boat.transform, obstacle.transform)) {
                    if (_collisionIds.Add(obstacle.id)) {
                        //Services.Get<IPlatform>().Log($"collision with {obstacle.id}");
                        boat.health -= 1;
                    }
                } else {
                    _collisionIds.Remove(obstacle.id);
                }
            }
		}
        
        private static void CheckHorizontalMove(Boat boat, int deltaX) {
            if (boat.velocity.x == 0f) {
                var targetLane = boat.lane + deltaX;

                if (targetLane is >= -1 and <= 1) {
                    boat.lane = targetLane;
                    boat.velocity.x = deltaX*boat.speed; // FIXME: possibilité de changer la vitesse en cours de changement de lane
                }
            } else {
                var targetX = boat.lane*SceneryController.LANE_DISTANCE;

                var moveLaneCompleted = (
                    boat.velocity.x < 0f && boat.transform.position.x <= targetX ||
                    boat.velocity.x > 0f && boat.transform.position.x >= targetX
                );
                if (moveLaneCompleted) {
                    boat.velocity.x = 0f;
                    boat.transform.position.x = targetX;
                }
            }
        }
    }
}
