using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    
    // Gère le spawn et le loot des munitions
    public static class AmmoSystem {
        public static void Init(GameState gameState) {
            gameState.PlayState.Ammos = new SwapbackArray<Cannonball>(16);
        }
        
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            
            TrySpawnAmmos(ref playState);
            CheckLoot(playState.Ammos, ref playState.Cannon, in playState.Boat);
            RemoveAmmosBehind(playState.Ammos, playState.Boat);
        }
        
        private static void TrySpawnAmmos(ref PlayState playState) {
            ref var cannon = ref playState.Cannon;
            var cannonConf = Services.Get<GameConf>().CannonConf;
            var regionConf = Services.Get<GameConf>().RegionConf;
            
            // On attend que le cooldown soit terminé pour en faire spawner une autre.
            if (cannon.AmmoSpawnCooldown > 0) {
                cannon.AmmoSpawnCooldown = Math.Max(0, cannon.AmmoSpawnCooldown - Clock.DeltaTime);
                return;
            }
            
            cannon.AmmoSpawnCooldown = cannonConf.AmmoSpawnFreq;

            // On n'en fait pas spawner si on ne peut pas transporter plus de munitions.
            if (cannon.AmmoCount == cannonConf.AmmoMax) return;

            // Règles de spawn en fonction de la distance.
            var spawnDistance = cannon.AmmoCount switch {
                0 => 30,
                < 5 => 100,
                _ => 500
            };
            spawnDistance = (int)(spawnDistance + playState.Boat.Position.Z)/CoreConfig.GridScale;
            
            // On ne fait pas spawner en dehors des limites de la région.
            if (spawnDistance > (regionConf.RegionDistance - regionConf.ZenDistance)/CoreConfig.GridScale) return;
            
            ref var entityGrid = ref playState.Region.EntityGrid;
            
            // On recherche un emplacement libre
            var emptyCells = new List<Vec2I32>(Enums.Count<LaneType>());
            for (var i = 0; i < entityGrid.Width; i++) {
                var coord = new Vec2I32(i, spawnDistance);
                var gridIndex = entityGrid.CoordsToIndex(coord);
                if (entityGrid.Items[gridIndex] == EntityType.None) {
                    emptyCells.Add(coord);
                }
            }
            
            // Spawn d'une nouvelle munition
            if (emptyCells.Count > 0) {
                var cellIndex = Prng.Roll(emptyCells.Count);
                var spawnCoord = emptyCells[cellIndex];
                var cannonball = Create(spawnCoord);
                playState.Ammos.Append(cannonball);
                
                var gridIndex = entityGrid.CoordsToIndex(spawnCoord);
                entityGrid.Items[gridIndex] = EntityType.Cannonball;
            }
        }
        
        private static Cannonball Create(Vec2I32 coords) {
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
                
                // S'il y a collision avec le bateau, on supprime la munition et incrémente le compteur de boulets.
                if (Collisions.CheckCollisionBoxes(ammo.Position, ammoBox, boat.Position, boatBox)) {
                    cannon.AmmoCount++;
                    ammos.RemoveAt(i);
                }
            }
        }
        
        /// <summary>
        /// Supprime les munitions une fois que le bateau les a dépassées.
        /// </summary>
        private static void RemoveAmmosBehind(SwapbackArray<Cannonball> ammos, Boat boat) {
            ammos.RemoveAll(ammo => ammo.Position.Z < boat.Position.Z - 10);
        }
    }
}
