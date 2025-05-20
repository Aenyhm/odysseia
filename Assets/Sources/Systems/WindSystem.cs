using System;
using Sources.States;
using Sources.Toolbox;

namespace Sources.Systems {
    public static class WindSystem {
        private static bool _changingAngle;
        private static int _targetAngle;
        
        public static void Init(out Wind wind) {
            wind = Blueprints.CreateWind();
        }
        
        public static void Update(ref Wind wind, in Boat boat, float dt) {
            if (_changingAngle) {
                if (Math.Abs(wind.Angle - _targetAngle) < float.Epsilon) {
                    wind.Angle = _targetAngle;
                    _changingAngle = false;
                    
                    wind.LastChangeDistance = boat.Distance;
                } else {
                    wind.Angle = Maths.MoveTowards(wind.Angle, _targetAngle, dt*boat.SpeedZ);
                }
            } else {
                if (boat.Distance > wind.LastChangeDistance + wind.Conf.ChangeDistance) {
                    _targetAngle = GetNewAngle(wind.Angle, wind.Conf.AngleMax);
                    _changingAngle = true;
                }
            }
        }
        
        private static int GetNewAngle(float currentAngle, int maxAngle) {
            var result = 0;
            
            if (currentAngle == 0) {
                var sign = Rnd.Next(2) == 0 ? -1 : 1;
                result = maxAngle*sign;
            }
            
            return result;
        }
    }
}
