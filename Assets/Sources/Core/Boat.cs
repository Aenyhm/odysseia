using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct BoatConf {
        public SpeedMaxConf SpeedMaxConf;
        public SailConf SailConf;
        public float PositionY;
        public float SpeedCollisionFactor;
        public float SpeedZStart;
        public float SpeedZMin;
        public float SpeedX;
        public byte HealthMax;
    }
    
    [Serializable]
    public struct SpeedMaxConf {
        public float Multiplier;
        public int DistanceStep;
        public int Min;
        public int Max;
    }
    
    [Serializable]
    public struct SailConf {
        public float AngleMax;
        public float WindwardAngleRange;
    }
    
    [Serializable]
    public struct Boat {
        public HashSet<int> CollisionIds;
        public Vec3F32 Position;  // Reset tous les 1000m pour changement de région
        public float SailAngle;
        public float SpeedZ;
        public int XSign;
        public byte Health;
        public LaneType LaneType;
        public bool SailWindward;
    }
    
    public static class BoatLogic {
        
        [Pure]
        public static float GetMaxSpeed(in SpeedMaxConf speedMaxConf, float distance) {
            var distanceFloored = Convert.ToInt64(distance);
            var distanceStep = distanceFloored/speedMaxConf.DistanceStep;
            var targetSpeed = (float)(speedMaxConf.Min*Math.Pow(speedMaxConf.Multiplier, distanceStep));
            
            return Math.Clamp(targetSpeed, speedMaxConf.Min, speedMaxConf.Max);
        }

        [Pure]
        public static bool IsSailWindward(in SailConf sailConf, float sailAngle, float windAngle) {
            var halfRange = sailConf.WindwardAngleRange/2;
            var min = Math.Max(-sailConf.AngleMax, windAngle - halfRange);
            var max = Math.Min(+sailConf.AngleMax, windAngle + halfRange);
            if (max - min < sailConf.WindwardAngleRange) {
                if (Maths.FloatEquals(min, -sailConf.AngleMax)) max = min + sailConf.WindwardAngleRange;
                if (Maths.FloatEquals(max, +sailConf.AngleMax)) min = max - sailConf.WindwardAngleRange;
            }
            
            return sailAngle >= min && sailAngle <= max;
        }
                
        [Pure]
        public static float MoveSailAngle(in SailConf sailConf, float angle) {
            return Math.Clamp(angle, -sailConf.AngleMax, +sailConf.AngleMax);
        }
    }
    
    public static class BoatSystem {
        public static Boat CreateBoat() {
            var boatConf = Services.Get<GameConf>().BoatConf;
            
            var boat = new Boat();
            boat.CollisionIds = new HashSet<int>();
            boat.Position.Y = boatConf.PositionY;
            boat.SpeedZ = boatConf.SpeedZStart;
            boat.Health = boatConf.HealthMax;
            boat.LaneType = LaneType.Center;
            
            return boat;
        }
        
        public static void Execute(ref GameState gameState, in GameInput input, float dt) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            var gameConf = Services.Get<GameConf>();
            var boatConf = gameConf.BoatConf;
            
            if (gameConf.EnableBoatCollisions) {
                CheckCollisions(ref boat, in playState.Region);
            }

            // Voile
            if (input.MouseButtonLeftDown) {
                var targetSailAngle = boat.SailAngle + input.MouseDeltaX;
                boat.SailAngle = BoatLogic.MoveSailAngle(boatConf.SailConf, targetSailAngle);
            }
            
            // Vitesse
            {
                var wind = playState.Wind;
                boat.SailWindward = BoatLogic.IsSailWindward(boatConf.SailConf, boat.SailAngle, wind.CurrentAngle);
                var speedDirection = boat.SailWindward ? 1 : -1;
                var targetSpeed = boat.SpeedZ + speedDirection*dt;
                var maxSpeed = BoatLogic.GetMaxSpeed(boatConf.SpeedMaxConf, playState.Distance);
                boat.SpeedZ = Math.Clamp(targetSpeed, boatConf.SpeedZMin, maxSpeed);
            }
            
            // Déplacement en avant
            var deltaZ = boat.SpeedZ*dt;
            boat.Position.Z += deltaZ;
            playState.Distance += deltaZ;
        }

        private static void CheckCollisions(ref Boat boat, in Region region) {
            var boatConf = Services.Get<GameConf>().BoatConf;
            var sizes = Services.Get<RendererConf>().Sizes;

            foreach (var e in region.Entities) {
                var pos = EntityLogic.GetPosition(e.Type, e.Coords);
                
                if (Collisions.CheckAabb(boat.Position, sizes[EntityType.Boat], pos, sizes[e.Type])) {
                    if (boat.CollisionIds.Add(e.Id)) {
                        boat.Health = (byte)Math.Max(0, boat.Health - 1);
                        var targetSpeed = boat.SpeedZ*boatConf.SpeedCollisionFactor;
                        boat.SpeedZ = Math.Max(boatConf.SpeedZMin, targetSpeed);
                    }
                } else {
                    boat.CollisionIds.Remove(e.Id);
                }
            }
        }
    }
}
