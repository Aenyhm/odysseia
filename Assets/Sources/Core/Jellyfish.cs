using System;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public struct JellyfishConf {
        public float Speed;
        public int SightDistance;
    }
    
    [Serializable]
    public struct JellyfishData {
        public Vec2I32 TargetCoord;
    }
    
    public static class JellyfishSystem {
        public static void Execute(ref GameState gameState) {
            var entities = gameState.PlayState.Region.Entities;
            ref var entityGrid = ref gameState.PlayState.Region.EntityGrid;
            var jellyfishConf = Services.Get<GameConf>().JellyfishConf;
            
            for (var i = 0; i < entities.Count; i++) {
                ref var e = ref entities.Items[i];
                if (e.Type != EntityType.Jellyfish) continue;
                
                if (
                    gameState.PlayState.Boat.Position.Z >= e.Position.Z - jellyfishConf.SightDistance &&
                    gameState.PlayState.Boat.Position.Z < e.Position.Z
                ) {
                    if (e.Coords[0] == e.JellyfishData.TargetCoord) {
                        var deltaMove = Prng.Roll(3) - 1;
                        var moveHorizontal = Prng.Chance(1, 2);
                        var targetCoord = new Vec2I32(moveHorizontal ? deltaMove : 0, moveHorizontal ? 0 : deltaMove);
                        
                        if (entityGrid.Items[entityGrid.CoordsToIndex(targetCoord)] == EntityType.None)
                            e.JellyfishData.TargetCoord = targetCoord;
                    } else {
                        var targetPos = EntityLogic.GetPosition(EntityType.Jellyfish, new[] { e.JellyfishData.TargetCoord });
                        e.Position.X = Maths.MoveTowards(e.Position.X, targetPos.X, Clock.DeltaTime*jellyfishConf.Speed);
                        e.Position.Z = Maths.MoveTowards(e.Position.Z, targetPos.Z, Clock.DeltaTime*jellyfishConf.Speed);
                        
                        if (e.Position == targetPos) {
                            entityGrid.Items[entityGrid.CoordsToIndex(e.JellyfishData.TargetCoord)] = EntityType.Jellyfish;
                            entityGrid.Items[entityGrid.CoordsToIndex(e.Coords[0])] = EntityType.None;
                            e.Coords[0] = e.JellyfishData.TargetCoord;
                        }
                    }
                }
            }
        }
    }
}
