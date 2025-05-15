using System;
using System.Diagnostics.Contracts;
using Sources.Toolbox;
using Random = System.Random;

namespace Sources.Core {
    public struct Wind {
        public float Angle;
        public float LastChangeDistance;
    }
    
    public static class WindSystem {
        private const int WIND_ANGLE_MAX_VALUE = 30;
        private const int WIND_CHANGE_DISTANCE = 60;
        private static bool _changingAngle;
        private static float _targetAngle;
        
        public static void Init(out Wind wind) {
            wind = new Wind();
        }
        
        public static void Update(ref Wind wind, in Boat boat, float dt) {
            if (_changingAngle) {
                if (Math.Abs(wind.Angle - _targetAngle) < float.Epsilon) {
                    wind.Angle = _targetAngle;
                    _changingAngle = false;
                    
                    wind.LastChangeDistance = boat.Position.Z;
                } else {
                    wind.Angle = Maths.MoveTowards(wind.Angle, _targetAngle, dt*boat.Speed);
                }
            } else {
                if (boat.Position.Z > wind.LastChangeDistance + WIND_CHANGE_DISTANCE) {
                    _targetAngle = GetNewAngle(wind.Angle, Services.Get<Random>());
                    _changingAngle = true;
                }
            }
        }
        
        [Pure]
        private static float GetNewAngle(float currentAngle, Random rnd) {
            float result = 0;
            
            if (currentAngle == 0) {
                var sign = rnd.Next(2) == 0 ? -1 : 1;
                result = WIND_ANGLE_MAX_VALUE*sign;
            }
            
            return result;
        }
    }
}
