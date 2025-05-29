using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct CannonConf {
        public float AmmoReloadTime;
        public float AmmoSpawnFreq;
        public float AmmoSpeedFactor;
        public int AmmoMax;
    }
    
    [Serializable]
    public struct Cannonball {
        public Vec3F32 Position;
        public Vec3F32 Velocity;
        public int Id;
        public bool Lootable;
        public bool Destroy;
    }
    
    [Serializable]
    public struct Cannon {
        public float ReloadCooldown;
        public float AmmoSpawnCooldown;
        public int AmmoCount;
    }
    
    public static class CannonballSystem {
        public static void Init(ref GameState gameState) {
            var cannonConf = Services.Get<GameConf>().CannonConf;
            gameState.PlayState.Entities.Cannonballs = new SwapbackArray<Cannonball>(cannonConf.AmmoMax);
        }
        
        public static void HandleCooldown(ref GameState gameState, in GameInput input, float dt) {
            ref var playState = ref gameState.PlayState;
            ref var cannon = ref playState.Cannon;
            
            if (input.Space && cannon.ReloadCooldown == 0f && cannon.AmmoCount > 0) {
                ref var cannonballs = ref playState.Entities.Cannonballs;

                var cannonball = CreateOnCannon(in playState.Boat);
                cannonballs.Add(cannonball);
                cannon.ReloadCooldown = Services.Get<GameConf>().CannonConf.AmmoReloadTime;
                cannon.AmmoCount--;
            }
            
            cannon.ReloadCooldown = Math.Max(0f, cannon.ReloadCooldown - dt);
        }
        
        public static void Execute(ref GameState gameState, float dt) {
            ref var playState = ref gameState.PlayState;
            ref var cannonballs = ref playState.Entities.Cannonballs;
            
            SpawnCannonballs(ref playState, dt);
            UpdateCannonballs(ref playState, dt);
            CheckLoot(ref playState);
            CheckCollisions(ref playState);

            cannonballs.RemoveAll(e => e.Destroy);
        }
        
        private static Cannonball CreateOnCannon(in Boat boat) {
            var cannonConf = Services.Get<GameConf>().CannonConf;
            var sizes = Services.Get<RendererConf>().Sizes;
            var offsetZ = sizes[EntityType.Boat].Z/2f + sizes[EntityType.Cannonball].Z/2f;
            
            var cannonball = new Cannonball();
            cannonball.Id = EntityLogic.NextId;
            cannonball.Position = new Vec3F32(boat.Position.X, 1.3f, boat.Position.Z + offsetZ);
            cannonball.Velocity.Z = boat.SpeedZ*cannonConf.AmmoSpeedFactor;

            return cannonball;
        }
        
        private static Cannonball CreateOnGround(Vec2I32 coords) {
            var cannonball = new Cannonball();
            cannonball.Id = EntityLogic.NextId;
            cannonball.Position = new Vec3F32((coords.X - 1)*CoreConfig.LaneDistance, 0.5f, coords.Y*CoreConfig.GridScale);
            cannonball.Lootable = true;
            
            return cannonball;
        }
        
        private static void SpawnCannonballs(ref PlayState playState, float dt) {
            ref var cannon = ref playState.Cannon;
            var cannonConf = Services.Get<GameConf>().CannonConf;
            var regionConf = Services.Get<GameConf>().RegionConf;
            
            if (cannon.AmmoSpawnCooldown > 0) {
                cannon.AmmoSpawnCooldown = Math.Max(0, cannon.AmmoSpawnCooldown - dt);
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
            
            if (spawnDistance > regionConf.RegionDistance - regionConf.ZenDistance) return;
            
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
                playState.Entities.Cannonballs.Add(cannonball);
                
                var gridIndex = entityGrid.CoordsToIndex(spawnCoord);
                entityGrid.Items[gridIndex] = EntityType.Cannonball;
            }
        }
        
        private static void UpdateCannonballs(ref PlayState playState, float dt) {
            ref var cannonballs = ref playState.Entities.Cannonballs;
            ref var boat = ref playState.Boat;
            
            for (var i = 0; i < cannonballs.Count; i++) {
                ref var cannonball = ref cannonballs.Items[i];
                
                if (cannonball.Position.Y < 0 || cannonball.Position.Z < boat.Position.Z - 40) {
                    cannonball.Destroy = true;
                }
                
                if (!cannonball.Lootable) {
                    cannonball.Position += cannonball.Velocity*dt;
                    cannonball.Velocity.Y -= 0.01f;
                }
            }
        }
        
        private static void CheckLoot(ref PlayState playState) {
            ref var cannonballs = ref playState.Entities.Cannonballs;
            ref var boat = ref playState.Boat;
            var sizes = Services.Get<RendererConf>().Sizes;
            var ammoMax = Services.Get<GameConf>().CannonConf.AmmoMax;

            for (var i = 0; i < cannonballs.Count; i++) {
                ref var cannonball = ref cannonballs.Items[i];
                
                if (!cannonball.Lootable) continue;

                if (Collisions.CheckAabb(cannonball.Position, sizes[EntityType.Cannonball], boat.Position, sizes[EntityType.Boat])) {
                    if (playState.Cannon.AmmoCount == ammoMax) break;
          
                    playState.Cannon.AmmoCount++;
                    cannonball.Destroy = true;
                }
            }
            
        }
        
        private static void CheckCollisions(ref PlayState playState) {
            ref var cannonballs = ref playState.Entities.Cannonballs;
            ref var entitiesToCheck = ref playState.Region.Entities;
                 
            var sizes = Services.Get<RendererConf>().Sizes;
            
            for (var index = entitiesToCheck.Count - 1; index >= 0; index--) {
                var e = entitiesToCheck[index];
                
                for (var i = 0; i < cannonballs.Count; i++) {
                    ref var cannonball = ref cannonballs.Items[i];
                    
                    if (cannonball.Destroy) continue;
                    
                    var pos = EntityLogic.GetPosition(e.Type, e.Coords);

                    if (Collisions.CheckAabb(cannonball.Position, sizes[EntityType.Cannonball], pos, sizes[e.Type])) {
                        cannonball.Destroy = true;
                        
                        if (EntityConf.DestroyableEntityTypes.Contains(e.Type)) {
                            entitiesToCheck.RemoveAt(index);

                            if (CoreConfig.EntityScoreValues.TryGetValue(e.Type, out var score)) {
                                playState.Score += score;
                            }
                        }
                    }
                }
            }
        }
    }
}
