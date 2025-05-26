using System;
using System.Collections.Generic;
using Sources.Mechanics;
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
            var boatConf = Services.Get<GameConf>().BoatConf;
            
            CheckCollisions(ref boat, in playState.Region);

            // Voile
            if (input.MouseButtonLeftDown) {
                var targetSailAngle = boat.SailAngle + input.MouseDeltaX;
                boat.SailAngle = BoatMechanics.MoveSailAngle(boatConf.SailConf, targetSailAngle);
            }
            
            // Vitesse
            {
                var wind = playState.Wind;
                boat.SailWindward = BoatMechanics.IsSailWindward(boatConf.SailConf, boat.SailAngle, wind.CurrentAngle);
                var speedDirection = boat.SailWindward ? 1 : -1;
                var targetSpeed = boat.SpeedZ + speedDirection*dt;
                var maxSpeed = BoatMechanics.GetMaxSpeed(boatConf.SpeedMaxConf, playState.Distance);
                boat.SpeedZ = Math.Clamp(targetSpeed, boatConf.SpeedZMin, maxSpeed);
            }
            
            // Déplacement en avant
            var deltaZ = boat.SpeedZ*dt;
            boat.Position.Z += deltaZ;
            playState.Distance += deltaZ;
        }

        private static void CheckCollisions(ref Boat boat, in Region region) {
            var boatConf = Services.Get<GameConf>().BoatConf;
            var boatSize = Services.Get<RendererConf>().Sizes[EntityType.Boat];

            foreach (var e in region.Entities) {
                if (Collisions.CheckAabb(boat.Position, boatSize, e.Position, e.Size)) {
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
