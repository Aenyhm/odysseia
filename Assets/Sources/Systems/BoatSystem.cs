using System;
using System.Collections.Generic;
using Sources.Mechanics;
using Sources.States;
using Sources.Toolbox;

namespace Sources.Systems {

    public static class BoatSystem {
        public static void Init(in RendererConf rendererConf, out Boat boat) {
            boat = Blueprints.CreateBoat(rendererConf.Sizes[EntityType.Boat]);
        }
        
        public static void Update(ref Boat boat, in GameInput input, in Wind wind, IEnumerable<Obstacle> obstacles, float dt) {
            CheckCollisions(ref boat, obstacles);

            // Voile
            if (input.MouseButtonLeftDown) {
                var targetSailAngle = boat.SailAngle + input.MouseDeltaX;
                boat.SailAngle = BoatMechanics.MoveSailAngle(boat.SailConf, targetSailAngle);
            }
            
            // Vitesse
            boat.SailWindward = BoatMechanics.IsSailWindward(boat.SailConf, boat.SailAngle, wind.Angle);
            var speedDirection = boat.SailWindward ? 1 : -1;
            var targetSpeed = boat.SpeedZ + speedDirection*dt;
            var maxSpeed = BoatMechanics.GetMaxSpeed(boat.SpeedMaxConf, boat.Distance);
            boat.SpeedZ = Math.Clamp(targetSpeed, boat.SpeedZMin, maxSpeed);
            
            // Déplacement
            boat.Position.X += boat.xSign*boat.SpeedX*dt;
            
            var deltaZ = boat.SpeedZ*dt;
            boat.Position.Z += deltaZ;
            boat.Distance += deltaZ;
            
            // Change Lane
            CheckHorizontalMove(ref boat, Convert.ToInt32(input.HorizontalAxis));
        }

        private static void CheckCollisions(ref Boat boat, IEnumerable<Obstacle> obstacles) {
            foreach (var obstacle in obstacles) {
                if (Collisions.CheckAabb(boat.Position, boat.Size, obstacle.Position, obstacle.Size)) {
                    if (boat.CollisionIds.Add(obstacle.Id)) {
                        boat.Health.Value = BoatMechanics.TakeDamage(boat.Health);
                        var targetSpeed = boat.SpeedZ*boat.SpeedCollisionFactor;
                        boat.SpeedZ = Math.Max(boat.SpeedZMin, targetSpeed);
                    }
                } else {
                    boat.CollisionIds.Remove(obstacle.Id);
                }
            }
		}

        private static void CheckHorizontalMove(ref Boat boat, int deltaX) {
            if (boat.xSign == 0) {
                boat.LaneType = LaneMechanics.GetDelta(boat.LaneType, deltaX);
                boat.xSign = deltaX;
            } else {
                var targetX = LaneMechanics.GetPosition(boat.LaneType, CoreConfig.LaneDistance);

                var moveLaneCompleted = (
                    boat.xSign == -1 && boat.Position.X <= targetX ||
                    boat.xSign == +1 && boat.Position.X >= targetX
                );
                if (moveLaneCompleted) {
                    boat.xSign = 0;
                    boat.Position.X = targetX;
                }
            }
        }
    }
}
