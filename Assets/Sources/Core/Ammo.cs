using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    public static class AmmoSystem {
        public static void Init(GameState gameState) {
            gameState.PlayState.Ammos = new SwapbackArray<Cannonball>(16);
        }
        
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            
            TrySpawnAmmos(ref playState);
            CheckLoot(playState.Ammos, ref playState.Cannon, in playState.Boat);
            
            // Remove passed
             for (var i = 0; i < playState.Ammos.Count; i++) {
                 var ammo = playState.Ammos.Items[i];
                 if (playState.Boat.Position.Z > ammo.Position.Z + 10) {
                     playState.Ammos.RemoveAt(i);
                 }
             }
        }
        
        private static void TrySpawnAmmos(ref PlayState playState) {
            ref var cannon = ref playState.Cannon;
            var cannonConf = Services.Get<GameConf>().CannonConf;
            var regionConf = Services.Get<GameConf>().RegionConf;
            
            if (cannon.AmmoSpawnCooldown > 0) {
                cannon.AmmoSpawnCooldown = Math.Max(0, cannon.AmmoSpawnCooldown - Clock.DeltaTime);
                return;
            }
            
            cannon.AmmoSpawnCooldown = cannonConf.AmmoSpawnFreq;

            if (cannon.AmmoCount == cannonConf.AmmoMax) return;

            var spawnDistance = cannon.AmmoCount switch {
                0 => 30,
                < 5 => 100,
                _ => 500
            };
            spawnDistance = (int)(spawnDistance + playState.Boat.Position.Z)/CoreConfig.GridScale;
            
            if (spawnDistance > (regionConf.RegionDistance - regionConf.ZenDistance)/CoreConfig.GridScale) return;
            
            ref var entityGrid = ref playState.Region.EntityGrid;
            
            var emptyCells = new List<Vec2I32>(Enums.Count<LaneType>());
            for (var i = 0; i < entityGrid.Width; i++) {
                var coord = new Vec2I32(i, spawnDistance);
                var gridIndex = entityGrid.CoordsToIndex(coord);
                if (entityGrid.Items[gridIndex] == EntityType.None) {
                    emptyCells.Add(coord);
                }
            }
            
            if (emptyCells.Count > 0) {
                var cellIndex = Prng.Roll(emptyCells.Count);
                var spawnCoord = emptyCells[cellIndex];
                var cannonball = CreateOnGround(spawnCoord);
                playState.Ammos.Append(cannonball);
                
                var gridIndex = entityGrid.CoordsToIndex(spawnCoord);
                entityGrid.Items[gridIndex] = EntityType.Cannonball;
            }
        }
        
        private static Cannonball CreateOnGround(Vec2I32 coords) {
            var cannonball = new Cannonball();
            cannonball.Id = EntityLogic.NextId;
            cannonball.Position = new Vec3F32((coords.X - 1)*CoreConfig.LaneDistance, 0.5f, coords.Y*CoreConfig.GridScale);
            
            return cannonball;
        }

        private static void CheckLoot(SwapbackArray<Cannonball> ammos, ref Cannon cannon, in Boat boat) {
            var ammoMax = Services.Get<GameConf>().CannonConf.AmmoMax;
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;
            var ammoBox = boxes[EntityType.Cannonball];
            var boatBox = boxes[EntityType.Boat];

            for (var i = ammos.Count - 1; i >= 0; i--) {
                if (cannon.AmmoCount == ammoMax) break;

                var ammo = ammos.Items[i];
                
                if (Collisions.CheckCollisionBoxes(ammo.Position, ammoBox, boat.Position, boatBox)) {
                    cannon.AmmoCount++;
                    ammos.RemoveAt(i);
                }
            }
        }
    }
}
