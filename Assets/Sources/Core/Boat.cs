using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public class Boat : Entity {
        public float speed;
        public int lane;
        public int health;
        public float sailAngle;
    }

    public static class BoatController {
        public const int HEALTH_MAX = 10;
        private const float SPEED_MIN = 10f;
        private const float SPEED_MAX = 30f;
        private const float SPEED_VARIATION = 1f;
        private const float COLLISION_SPEED_DECREASE = 5f;
        private const float SAIL_ANGLE_MAX = 30f;
        private const float SAIL_WINDWARD_ANGLE_RANGE = 20f;

        private static readonly HashSet<int> _collisionIds = new();
        private static int _changingLaneSign;

        public static Boat Create() {
            var boat = EntityManager.Create<Boat>();
            boat.speed = 10;
            boat.transform.position.y = 0.5f;
            boat.health = HEALTH_MAX;
            
            Services.Get<IPlatform>().AddEntityView(boat);
            
            return boat;
        }

        public static void Update(float dt, GameInput input) {
            var gs = Services.Get<GameState>();

            var boat = gs.boat;

			CheckCollisions(boat, gs.region.activeObstacles);
            
            CheckHorizontalMove(boat, Convert.ToInt32(input.HorizontalAxis));
            
            // Changer l'angle de la voile
            if (input.MouseButtonLeftDown) {
                var targetSailAngle = boat.sailAngle + input.MouseDeltaX;
                boat.sailAngle = Math.Clamp(targetSailAngle, -SAIL_ANGLE_MAX, +SAIL_ANGLE_MAX);
            }
            
            // Vitesse
            var sailWindward = IsSailWindward(boat, gs.wind);
            var speedDirection = sailWindward ? 1 : -1;
            var targetSpeed = boat.speed + speedDirection*SPEED_VARIATION*dt;
            boat.speed = Math.Clamp(targetSpeed, SPEED_MIN, SPEED_MAX);
            
            // Position
            boat.transform.position.x += _changingLaneSign*boat.speed*dt;
            boat.transform.position.z += boat.speed*dt;
        }
        
        public static bool IsSailWindward(Boat boat, Wind wind) {
            // Voile au vent
			// vent   0 = voile [-10, +10]
			// vent -30 = voile [-30, -10]
			// vent +30 = voile [+10, +30]
			const float halfRange = SAIL_WINDWARD_ANGLE_RANGE/2;
            var minSailAngle = wind.angle - halfRange;
            var maxSailAngle = wind.angle + halfRange;

            // Si dépasse les bornes, on recentre pour garder le range constant
            if (minSailAngle < -SAIL_ANGLE_MAX) {
                minSailAngle = -SAIL_ANGLE_MAX;
                maxSailAngle = -SAIL_ANGLE_MAX + SAIL_WINDWARD_ANGLE_RANGE;
            } else if (maxSailAngle > SAIL_ANGLE_MAX) {
                maxSailAngle = SAIL_ANGLE_MAX;
                minSailAngle = SAIL_ANGLE_MAX - SAIL_WINDWARD_ANGLE_RANGE;
            }
            
            return boat.sailAngle >= minSailAngle && boat.sailAngle <= maxSailAngle;
        }

		private static void CheckCollisions(Boat boat, List<Entity> obstacles) {
            foreach (var obstacle in obstacles) {
                if (Collisions.CheckAabb(boat.transform, obstacle.transform)) {
                    if (_collisionIds.Add(obstacle.id)) {
                        boat.health -= 1;
                        var targetSpeed = boat.speed - COLLISION_SPEED_DECREASE;
                        boat.speed = Math.Max(SPEED_MIN, targetSpeed);
                    }
                } else {
                    _collisionIds.Remove(obstacle.id);
                }
            }
		}
        
        private static void CheckHorizontalMove(Boat boat, int deltaX) {
            if (_changingLaneSign == 0) {
                var targetLane = boat.lane + deltaX;

                if (targetLane is >= -1 and <= 1) {
                    boat.lane = targetLane;
                    _changingLaneSign = deltaX;
                }
            } else {
                var targetX = boat.lane*SceneryController.LANE_DISTANCE;

                var moveLaneCompleted = (
                    _changingLaneSign == -1 && boat.transform.position.x <= targetX ||
                    _changingLaneSign == +1 && boat.transform.position.x >= targetX
                );
                if (moveLaneCompleted) {
                    _changingLaneSign = 0;
                    boat.transform.position.x = targetX;
                }
            }
        }
    }
}
