using System;
using System.Diagnostics.Contracts;
using Sources.States;

namespace Sources.Mechanics {
    public static class BoatMechanics {
        
        [Pure]
        public static float GetMaxSpeed(in SpeedMaxConf speedMaxConf, float distance) {
            var distanceFloored = Convert.ToInt64(distance);
            var distanceStep = distanceFloored/speedMaxConf.DistanceStep;
            var targetSpeed = (float)(speedMaxConf.Min*Math.Pow(speedMaxConf.Multiplier, distanceStep));
            
            return Math.Clamp(targetSpeed, speedMaxConf.Min, speedMaxConf.Max);
        }

        [Pure]
        public static bool IsSailWindward(in SailConf sailConf, float sailAngle, float windAngle) {
            var halfRange = sailConf.WindwardAngleRange/2;
            var min = Math.Max(-sailConf.AngleMax, windAngle - halfRange);
            var max = Math.Min(+sailConf.AngleMax, windAngle + halfRange);
            if (max - min < sailConf.WindwardAngleRange) {
                if (Math.Abs(min - -sailConf.AngleMax) < float.Epsilon) max = min + sailConf.WindwardAngleRange;
                if (Math.Abs(max - +sailConf.AngleMax) < float.Epsilon) min = max - sailConf.WindwardAngleRange;
            }
            
            return sailAngle >= min && sailAngle <= max;
        }
                
        [Pure]
        public static float MoveSailAngle(in SailConf sailConf, float angle) {
            return Math.Clamp(angle, -sailConf.AngleMax, +sailConf.AngleMax);
        }
        
        [Pure]
        public static byte TakeDamage(in Health health) {
            return (byte)Math.Clamp(health.Value - 1, 0, health.Max);
        }
    }
}
