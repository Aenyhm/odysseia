using System;
using Sources.Mechanics;
using Sources.Toolbox;

namespace Sources.Core {
    public enum LaneType : byte { Left, Center, Right }

    public static class ChangeLaneSystem {
        public static void Execute(ref GameState gameState, in GameInput input, float dt) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            
            if (boat.XSign == 0) {
                var deltaX = Convert.ToInt32(input.HorizontalAxis);
                if (deltaX == 0) {
                    deltaX = GetForcedDeltaX(in playState);
                }
                
                boat.LaneType = LaneMechanics.GetDelta(boat.LaneType, deltaX);
                boat.XSign = deltaX;
            } else {
                var targetX = LaneMechanics.GetPosition(boat.LaneType, CoreConfig.LaneDistance);
                
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
                if (e.Type == EntityType.Mermaid) {
                    if (
                        e.Position.Z > playState.Boat.Position.Z &&
                        e.Position.Z - playState.Boat.Position.Z < CoreConfig.MermaidEffectDistance
                    ) {
                        if (e.LaneType < boat.LaneType) result = -1;
                        else if (e.LaneType > boat.LaneType) result = +1;
                        break;
                    }
                }
            }
            
            return result;
        }
    }
}
