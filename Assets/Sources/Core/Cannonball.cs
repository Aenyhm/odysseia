using System;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct Cannonball {
        public Vec3F32 Position;
        public Vec3F32 Velocity;
        public int Id;
    }
    
    public static class CannonballSystem {
                
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            var cannonballs = playState.Cannonballs;
            
            UpdateCannonballs(cannonballs);
            CheckCollisions(gameState, cannonballs, playState.Region.Entities);
        }
        
        private static void UpdateCannonballs(SwapbackArray<Cannonball> cannonballs) {
            for (var i = cannonballs.Count - 1; i >= 0; i--) {
                ref var cannonball = ref cannonballs.Items[i];
                
                if (cannonball.Position.Y < 0) {
                    cannonballs.RemoveAt(i);
                } else {
                    cannonball.Position += cannonball.Velocity*Clock.DeltaTime;
                    cannonball.Velocity.Y -= 0.01f;
                }
            }
        }
        
        private static void CheckCollisions(
            GameState gameState, SwapbackArray<Cannonball> cannonballs, SwapbackArray<Entity> entities
        ) {
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;
            var cannonballSize = boxes[EntityType.Cannonball];
            
            for (var index = 0; index < entities.Count; index++) {
                ref var e = ref entities.Items[index];
                if (e.Destroy) continue;
                
                var entityBox = boxes[e.Type];

                for (var i = cannonballs.Count - 1; i >= 0; i--) {
                    var cannonball = cannonballs.Items[i];
                 
                    if (Collisions.CheckCollisionBoxes(cannonball.Position, cannonballSize, e.Position, entityBox)) {
                        cannonballs.RemoveAt(i);
                        
                        if (EntityConf.DestroyableEntityTypes.Contains(e.Type)) {
                            e.Destroy = true;

                            if (EntityConf.EntityScoreValues.TryGetValue(e.Type, out var score)) {
                                ScoreLogic.Add(gameState, score);
                            }
                        }
                    }
                }
            }
        }
    }
}
