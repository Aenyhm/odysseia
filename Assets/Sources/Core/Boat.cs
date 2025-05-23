using System;
using System.Collections.Generic;
using Sources.Configuration;
using Sources.Mechanics;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public struct Boat {
        public BoatConf Conf;
        public HashSet<int> CollisionIds;
        public Vec3F32 Size;
        public Vec3F32 Position;  // Reset tous les 1000m pour changement de région
        public float SailAngle;
        public float SpeedZ;
        public int xSign;
        public byte Health;
        public LaneType LaneType;
        public bool SailWindward;
    }
    
    public static class BoatSystem {
        
        public static Boat CreateBoat() {
            var boat = new Boat();
            boat.Conf = CoreConfig.BoatConf;
            boat.CollisionIds = new HashSet<int>();
            boat.Size = Services.Get<RendererConf>().Sizes[EntityType.Boat];
            boat.Position.Y = boat.Conf.PositionY;
            boat.SpeedZ = boat.Conf.SpeedZStart;
            boat.Health = boat.Conf.HealthMax;
            boat.LaneType = LaneType.Center;
            
            return boat;
        }
        
        public static void Execute(ref PlayState playState, in GameInput input, float dt) {
            ref var boat = ref playState.Boat;

            CheckCollisions(ref boat, in playState.Region);

            // Voile
            if (input.MouseButtonLeftDown) {
                var targetSailAngle = boat.SailAngle + input.MouseDeltaX;
                boat.SailAngle = BoatMechanics.MoveSailAngle(boat.Conf.SailConf, targetSailAngle);
            }
            
            // Vitesse
            {
                var wind = playState.Wind;
                boat.SailWindward = BoatMechanics.IsSailWindward(boat.Conf.SailConf, boat.SailAngle, wind.CurrentAngle);
                var speedDirection = boat.SailWindward ? 1 : -1;
                var targetSpeed = boat.SpeedZ + speedDirection*dt;
                var maxSpeed = BoatMechanics.GetMaxSpeed(boat.Conf.SpeedMaxConf, playState.Distance);
                boat.SpeedZ = Math.Clamp(targetSpeed, boat.Conf.SpeedZMin, maxSpeed);
            }
            
            // Déplacement
            boat.Position.X += boat.xSign*boat.Conf.SpeedX*dt;
            
            var deltaZ = boat.SpeedZ*dt;
            boat.Position.Z += deltaZ;
            playState.Distance += deltaZ;
            
            // Change Lane
            CheckHorizontalMove(ref boat, Convert.ToInt32(input.HorizontalAxis));
        }

        private static void CheckCollisions(ref Boat boat, in Region region) {
            foreach (var obstacles in region.ObstaclesByType.Values) {
                foreach (var obstacle in obstacles) {
                    if (Collisions.CheckAabb(boat.Position, boat.Size, obstacle.Position, obstacle.Size)) {
                        if (boat.CollisionIds.Add(obstacle.Id)) {
                            boat.Health = (byte)Math.Max(0, boat.Health - 1);
                            var targetSpeed = boat.SpeedZ*boat.Conf.SpeedCollisionFactor;
                            boat.SpeedZ = Math.Max(boat.Conf.SpeedZMin, targetSpeed);
                        }
                    } else {
                        boat.CollisionIds.Remove(obstacle.Id);
                    }
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
