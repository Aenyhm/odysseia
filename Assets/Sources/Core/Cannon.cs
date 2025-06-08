using System;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct CannonConf {
        public float AmmoReloadTime;
        public float AmmoSpawnFreq;
        public float AmmoSpeed;
        public int AmmoMax;
        public int AmmoInitialCount;
    }

    [Serializable]
    public struct Cannon {
        public float ReloadCooldown;
        public float AmmoSpawnCooldown;
        public int AmmoCount;
    }
    
    public static class CannonSystem {
        public static void Init(GameState gameState) {
            gameState.PlayState.Cannonballs = new SwapbackArray<Cannonball>(4);
            gameState.PlayState.Cannonballs.Reset();
            gameState.PlayState.Cannon.AmmoCount = Services.Get<GameConf>().CannonConf.AmmoInitialCount;
        }
        
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            ref var cannon = ref playState.Cannon;
            
            if (gameState.PlayerActions.Shoot && cannon.ReloadCooldown == 0f && cannon.AmmoCount > 0) {
                var cannonballs = playState.Cannonballs;

                var cannonball = CreateOnCannon(in playState.Boat);
                cannonballs.Append(cannonball);
                cannon.ReloadCooldown = Services.Get<GameConf>().CannonConf.AmmoReloadTime;
                cannon.AmmoCount--;
            }
            
            cannon.ReloadCooldown = Math.Max(0f, cannon.ReloadCooldown - Clock.DeltaTime);
        }
        
        private static Cannonball CreateOnCannon(in Boat boat) {
            var cannonConf = Services.Get<GameConf>().CannonConf;
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;
            var offsetZ = boxes[EntityType.Boat].Max.Z/2f + boxes[EntityType.Cannonball].Max.Z/2f;
            
            var cannonball = new Cannonball();
            cannonball.Id = EntityLogic.NextId;
            cannonball.Position = new Vec3F32(boat.Position.X, 1.3f, boat.Position.Z + offsetZ);
            cannonball.Velocity.Z = cannonConf.AmmoSpeed;

            return cannonball;
        }
    }
}
