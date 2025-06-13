using System;
using System.Diagnostics.Contracts;
using Sources.Toolbox;

namespace Sources.Core {
    public enum LaneType : byte { Left, Center, Right }
    
    public static class LaneLogic {
        [Pure]
        public static LaneType GetDelta(LaneType laneType, int delta) {
            var index = (int)laneType + delta;
            
            return (LaneType)Math.Clamp(index, 0, Enums.Count<LaneType>() - 1);
        }

        public static float GetPosition(float x) => (x - 1)*CoreConfig.LaneDistance;
    }

    public static class ChangeLaneSystem {
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            
            if (boat.XSign == 0) {
                // Choix de changement de lane
                var deltaX = boat.CharmedById != 0 ?
                    GetForcedDeltaX(in playState) :
                    Convert.ToInt32(gameState.PlayerActions.SideMove);

                boat.LaneType = LaneLogic.GetDelta(boat.LaneType, deltaX);
                boat.XSign = deltaX;
            } else {
                // Déplacement
                var targetX = LaneLogic.GetPosition((int)boat.LaneType);
                
                var boatConf = Services.Get<GameConf>().BoatConf;

                boat.Position.X = Maths.MoveTowards(boat.Position.X, targetX, boatConf.SpeedX*Clock.DeltaTime);
                
                var moveLaneCompleted = (
                    boat.XSign == -1 && boat.Position.X <= targetX ||
                    boat.XSign == +1 && boat.Position.X >= targetX
                );
                if (moveLaneCompleted) {
                    boat.XSign = 0;
                }
            }
        }
        
        // Note: Spécifique à la sirène ; devrait être dans son fichier.
        private static int GetForcedDeltaX(in PlayState playState) {
            var result = 0;
            
            var boat = playState.Boat;
            var mermaids = playState.Region.EntitiesByType[EntityType.Mermaid];
        
            foreach (var e in mermaids) {
                if (boat.CharmedById == e.Id) {
                    var laneType = (LaneType)e.Coords[0].X;
                    
                    if (laneType < boat.LaneType) result = -1;
                    else if (laneType > boat.LaneType) result = +1;
                    break;
                }
            }
            
            return result;
        }
    }
}
