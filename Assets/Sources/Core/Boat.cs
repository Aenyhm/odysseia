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
        public float TurnSpeed;
    }
    
    [Serializable]
    public struct Boat {
        public HashSet<int> CollisionIds;
        public Vec3F32 Position;  // Reset tous les 1000m pour changement de région
        public float Distance;
        public float SailAngle;
        public float SpeedZ;
        public float MeterDelta;
        public int XSign;
        public int CharmedById;
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
        public static Vec2F32 GetWindwardAngle(in SailConf sailConf, float windAngle) {
            var halfRange = sailConf.WindwardAngleRange/2;
            var min = Math.Max(-sailConf.AngleMax, windAngle - halfRange);
            var max = Math.Min(+sailConf.AngleMax, windAngle + halfRange);
            if (max - min < sailConf.WindwardAngleRange) {
                if (Maths.FloatEquals(min, -sailConf.AngleMax)) max = min + sailConf.WindwardAngleRange;
                if (Maths.FloatEquals(max, +sailConf.AngleMax)) min = max - sailConf.WindwardAngleRange;
            }
            
            return new Vec2F32(min, max);
        }

        [Pure]
        public static float MoveSailAngle(in SailConf sailConf, float angle) {
            return Math.Clamp(angle, -sailConf.AngleMax, +sailConf.AngleMax);
        }
    }
    
    public static class BoatSystem {
        public static void Init(GameState gameState) {
            ref var boat = ref gameState.PlayState.Boat;
            var boatConf = Services.Get<GameConf>().BoatConf;
            
            boat.CollisionIds = new HashSet<int>();
            boat.Position.Y = boatConf.PositionY;
            boat.SpeedZ = boatConf.SpeedZStart;
            boat.Health = boatConf.HealthMax;
            boat.LaneType = LaneType.Center;
        }
        
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            var gameConf = Services.Get<GameConf>();
            var boatConf = gameConf.BoatConf;
            
            var checkCollisions = gameConf.EnableBoatCollisions;
            #if !UNITY_EDITOR
            checkCollisions = true;
            #endif
            if (checkCollisions) {
                CheckCollisions(ref boat, playState.Region.Entities);
            }

            // Voile
            if (!Maths.FloatEquals(gameState.PlayerActions.DeltaSail, 0)) {
                var targetSailAngle = boat.SailAngle + gameState.PlayerActions.DeltaSail*boatConf.SailConf.TurnSpeed;
                boat.SailAngle = BoatLogic.MoveSailAngle(boatConf.SailConf, targetSailAngle);
            }
            
            // Vitesse
            {
                var wind = playState.Wind;
                var winwardAngle = BoatLogic.GetWindwardAngle(boatConf.SailConf, wind.CurrentAngle);
                boat.SailWindward = boat.SailAngle >= winwardAngle.X && boat.SailAngle <= winwardAngle.Y;
                
                var speedDirection = boat.SailWindward ? 1 : -1;
                var targetSpeed = boat.SpeedZ + speedDirection*Clock.DeltaTime;
                var maxSpeed = BoatLogic.GetMaxSpeed(boatConf.SpeedMaxConf, boat.Distance);
                boat.SpeedZ = Math.Clamp(targetSpeed, boatConf.SpeedZMin, maxSpeed);
            }
            
            // Déplacement en avant
            var deltaZ = boat.SpeedZ*Clock.DeltaTime;
            boat.Position.Z += deltaZ;
            boat.Distance += deltaZ;
            
            ComputeDistanceScore(gameState, deltaZ);
        }
        
        private static void ComputeDistanceScore(GameState gameState, float deltaZ) {
            ref var boat = ref gameState.PlayState.Boat;
            
            var total = boat.MeterDelta + deltaZ;
            
            var integer = (int)total;
            var fractional = total - integer;
            
            boat.MeterDelta = fractional;
            ScoreLogic.Add(gameState, integer);
        }

        private static void CheckCollisions(ref Boat boat, SwapbackArray<Entity> entities) {
            var boatConf = Services.Get<GameConf>().BoatConf;
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;

            foreach (var e in entities) {
                if (EntityConf.ObstacleTypes.Contains(e.Type)) {
                    if (Collisions.CheckCollisionBoxes(boat.Position, boxes[EntityType.Boat], e.Position, boxes[e.Type])) {
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
}
