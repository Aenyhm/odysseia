using System;
using Sources.Configuration;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct Wind {
        public WindConf Conf;
        public float CurrentAngle;
        public float TargetAngle;
        public float LastChangeDistance;
    }
    
    public static class WindSystem {
        public static Wind CreateWind() {
            var wind = new Wind();
            wind.Conf = CoreConfig.WindConf;
            return wind;
        }
        
        public static void Execute(ref PlayState playState, float dt) {
            ref var wind = ref playState.Wind;
            var boat = playState.Boat;

            if (Maths.FloatEquals(wind.CurrentAngle, wind.TargetAngle)) {
                if (playState.Distance > wind.LastChangeDistance + wind.Conf.ChangeDistance) {
                    wind.TargetAngle = GetNewAngle(wind.CurrentAngle, wind.Conf.AngleMax);
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
