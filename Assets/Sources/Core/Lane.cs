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
        public static void Execute(ref GameState gameState, in GameInput input, float dt) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            
            if (boat.XSign == 0) {
                var deltaX = boat.CharmedById != 0 ?
                    GetForcedDeltaX(in playState) :
                    Convert.ToInt32(input.HorizontalAxis);

                boat.LaneType = LaneLogic.GetDelta(boat.LaneType, deltaX);
                boat.XSign = deltaX;
            } else {
                var targetX = LaneLogic.GetPosition((int)boat.LaneType);
                
                var boatConf = Services.Get<GameConf>().BoatConf;

                boat.Position.X = Maths.MoveTowards(boat.Position.X, targetX, boatConf.SpeedX*dt);
                
                var moveLaneCompleted = (
                    boat.XSign == -1 && boat.Position.X <= targetX ||
                    boat.XSign == +1 && boat.Position.X >= targetX
                );
                if (moveLaneCompleted) {
                    boat.XSign = 0;
                }
            }
        }
        
        private static int GetForcedDeltaX(in PlayState playState) {
            var result = 0;
            
            var boat = playState.Boat;
        
            foreach (var e in playState.Region.Entities) {
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
