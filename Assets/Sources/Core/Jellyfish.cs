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
        public static void Execute(GameState gameState) {
            var jellyfishes = gameState.PlayState.Region.EntitiesByType[EntityType.Jellyfish];
            ref var entityGrid = ref gameState.PlayState.Region.EntityGrid;
            var jellyfishConf = Services.Get<GameConf>().JellyfishConf;
            
            for (var i = 0; i < jellyfishes.Count; i++) {
                ref var e = ref jellyfishes.Items[i];
                
                // La méduse se déplace aléatoirement d'une case libre autour d'elle dans le voisinage de Von Neumann.
                if (
                    gameState.PlayState.Boat.Position.Z >= e.Position.Z - jellyfishConf.SightDistance &&
                    gameState.PlayState.Boat.Position.Z < e.Position.Z
                ) {
                    if (e.Coords[0] == e.JellyfishData.TargetCoord) {
                        // Choix de case
                        var deltaMove = Prng.Roll(3) - 1;
                        var moveHorizontal = Prng.Chance(1, 2);
                        var targetCoord = new Vec2I32(moveHorizontal ? deltaMove : 0, moveHorizontal ? 0 : deltaMove);
                        
                        if (entityGrid.Items[entityGrid.CoordsToIndex(targetCoord)] == EntityType.None)
                            e.JellyfishData.TargetCoord = targetCoord;
                    } else {
                        // Déplacement
                        var targetPos = EntityLogic.GetPosition(EntityType.Jellyfish, new[] { e.JellyfishData.TargetCoord });
                        e.Position.X = Maths.MoveTowards(e.Position.X, targetPos.X, Clock.DeltaTime*jellyfishConf.Speed);
                        e.Position.Z = Maths.MoveTowards(e.Position.Z, targetPos.Z, Clock.DeltaTime*jellyfishConf.Speed);
                        
                        if (e.Position == targetPos) {
                            // Fin de déplacement, mise à jour de la grille.
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
