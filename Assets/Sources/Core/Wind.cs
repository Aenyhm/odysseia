using System;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct WindConf {
        public int AngleMax;
        public int ChangeDistance;
    }
    
    [Serializable]
    public struct Wind {
        public float CurrentAngle;
        public float TargetAngle;
        public float LastChangeDistance;
    }
    
    public static class WindSystem {
        public static void Execute(ref GameState gameState, float dt) {
            ref var playState = ref gameState.PlayState;
            ref var wind = ref playState.Wind;
            var boat = playState.Boat;

            if (Maths.FloatEquals(wind.CurrentAngle, wind.TargetAngle)) {
                var windConf = Services.Get<GameConf>().WindConf;

                if (playState.Distance > wind.LastChangeDistance + windConf.ChangeDistance) {
                    wind.TargetAngle = GetNewAngle(wind.CurrentAngle, windConf.AngleMax);
                    wind.LastChangeDistance = playState.Distance;
                }
            } else {
                wind.CurrentAngle = Maths.MoveTowards(wind.CurrentAngle, wind.TargetAngle, dt*boat.SpeedZ);
            }
        }
        
        private static int GetNewAngle(float currentAngle, int maxAngle) {
            var result = 0;
            
            if (currentAngle == 0) {
                var sign = Prng.Roll(2) == 0 ? -1 : 1;
                result = maxAngle*sign;
            }
            
            return result;
        }
    }
}
