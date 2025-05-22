using System;
using Sources.Mechanics;
using Sources.States;
using Sources.Toolbox;

namespace Sources.Systems {

    public class BoatSystem : AbstractSystem {
        private readonly Vec3F32 _boatSize;
        
        public BoatSystem(in RendererConf rendererConf) {
            _boatSize = rendererConf.Sizes[EntityType.Boat];
        }
        
        public override void Init(ref GameState gameState) {
            gameState.Boat = Blueprints.CreateBoat(_boatSize);
        }
        
        public override void Update(ref GameState gameState, in GameInput input, float dt) {
            ref var boat = ref gameState.Boat;

            if (gameState.GameMode == GameMode.Run) {
                var wind = gameState.Wind;
                var region = gameState.Region;

                CheckCollisions(ref gameState, in region);

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
            } else if (gameState.GameMode == GameMode.GameOver) {
                boat.SailWindward = false;
            }
        }

        private static void CheckCollisions(ref GameState gameState, in Region region) {
            ref var boat = ref gameState.Boat;
            
            foreach (var obstacles in region.ObstaclesByType.Values) {
                foreach (var obstacle in obstacles) {
                    if (Collisions.CheckAabb(boat.Position, boat.Size, obstacle.Position, obstacle.Size)) {
                        if (boat.CollisionIds.Add(obstacle.Id)) {
                            boat.Health.Value = BoatMechanics.TakeDamage(boat.Health);
                            if (boat.Health.Value == 0) {
                                gameState.TotalCoinCount += gameState.RunCoinCount;
                                gameState.GameMode = GameMode.GameOver;
                            }
                            var targetSpeed = boat.SpeedZ*boat.SpeedCollisionFactor;
                            boat.SpeedZ = Math.Max(boat.SpeedZMin, targetSpeed);
                        }
                    } else {
                        boat.CollisionIds.Remove(obstacle.Id);
                    }
                }
            }

            for (var i = region.Coins.Count - 1; i >= 0; i--) {
                var coin = region.Coins[i];
                if (Collisions.CheckAabb(boat.Position, boat.Size, coin.Position, coin.Size)) {
                    gameState.RunCoinCount++;
                    region.Coins.RemoveAt(i);
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
