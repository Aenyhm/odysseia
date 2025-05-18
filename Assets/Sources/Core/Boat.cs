using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources.Toolbox;

namespace Sources.Core {
    public struct Boat {
        public Vec3F32 Size;
        public Vec3F32 Position;
        public float Distance;
        public float SpeedZ;
        public float SailAngle;
        public LaneType LaneType;
        public byte Health;
    }

    public static class BoatSystem {
        // TODO: Config
        private const float SPEED_MIN = 10f;
        private const float SPEED_MAX = 30f;
        private const float SPEED_X = 30f;
        private const float SAIL_ANGLE_MAX = 30f;
        private const float SPEED_VARIATION = 1f;
        private const float COLLISION_SPEED_DECREASE = 5f;
        private const float SAIL_WINDWARD_ANGLE_RANGE = 20f;
        public const int HEALTH_MAX = 10;
        
        private static readonly HashSet<int> _collisionIds = new();
        private static int _changingLaneSign;
        
        public static void Init(in Conf conf, out Boat boat) {
            boat = new Boat();
            boat.Position.Y = 0.5f;
            boat.Size = conf.Sizes[EntityType.Boat];
            boat.SpeedZ = SPEED_MIN;
            boat.SailAngle = 0f;
            boat.Health = HEALTH_MAX;
            boat.LaneType = LaneType.Center;
        }
        
        public static void Update(ref Boat boat, in GameInput input, in Wind wind, IEnumerable<Obstacle> obstacles, float dt) {
            CheckCollisions(ref boat, obstacles);

            // Voile
            if (input.MouseButtonLeftDown) {
                var targetSailAngle = boat.SailAngle + input.MouseDeltaX;
                boat.SailAngle = Math.Clamp(targetSailAngle, -SAIL_ANGLE_MAX, +SAIL_ANGLE_MAX);
            }
            
            // Vitesse
            var sailWindward = IsSailWindward(boat.SailAngle, wind.Angle);
            var speedDirection = sailWindward ? 1 : -1;
            var targetSpeed = boat.SpeedZ + speedDirection*SPEED_VARIATION*dt;
            boat.SpeedZ = Math.Clamp(targetSpeed, SPEED_MIN, SPEED_MAX);
            
            // Déplacement
            boat.Position.X += _changingLaneSign*SPEED_X*dt;
            boat.Position.Z += boat.SpeedZ*dt; // Reset tous les 1000m pour changement de région
            boat.Distance += boat.SpeedZ*dt;
            
            // Change Lane
            CheckHorizontalMove(ref boat, Convert.ToInt32(input.HorizontalAxis));
        }

        private static void CheckCollisions(ref Boat boat, IEnumerable<Obstacle> obstacles) {
            foreach (var obstacle in obstacles) {
                if (Collisions.CheckAabb(boat.Position, boat.Size, obstacle.Position, obstacle.Size)) {
                    if (_collisionIds.Add(obstacle.Id)) {
                        boat.Health = (byte)Math.Max(0, boat.Health - 1);
                        var targetSpeed = boat.SpeedZ - COLLISION_SPEED_DECREASE;
                        boat.SpeedZ = Math.Max(SPEED_MIN, targetSpeed);
                    }
                } else {
                    _collisionIds.Remove(obstacle.Id);
                }
            }
		}

        private static void CheckHorizontalMove(ref Boat boat, int deltaX) {
            if (_changingLaneSign == 0) {
                boat.LaneType = LaneHelper.GetDelta(boat.LaneType, deltaX);
                _changingLaneSign = deltaX;
            } else {
                var targetX = LaneHelper.GetPosition(boat.LaneType);

                var moveLaneCompleted = (
                    _changingLaneSign == -1 && boat.Position.X <= targetX ||
                    _changingLaneSign == +1 && boat.Position.X >= targetX
                );
                if (moveLaneCompleted) {
                    _changingLaneSign = 0;
                    boat.Position.X = targetX;
                }
            }
        }
        
        [Pure]
        public static bool IsSailWindward(float sailAngle, float windAngle) {
            const float halfRange = SAIL_WINDWARD_ANGLE_RANGE/2;
            var min = Math.Max(-SAIL_ANGLE_MAX, windAngle - halfRange);
            var max = Math.Min(+SAIL_ANGLE_MAX, windAngle + halfRange);
            if (max - min < SAIL_WINDWARD_ANGLE_RANGE) {
                if (Math.Abs(min - (-SAIL_ANGLE_MAX)) < float.Epsilon) max = min + SAIL_WINDWARD_ANGLE_RANGE;
                if (Math.Abs(max - +SAIL_ANGLE_MAX) < float.Epsilon) min = max - SAIL_WINDWARD_ANGLE_RANGE;
            }
            
            return sailAngle >= min && sailAngle <= max;
        }
    }
}
