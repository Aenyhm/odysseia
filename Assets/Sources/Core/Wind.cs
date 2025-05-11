using System;
using Sources.Toolbox;
using Random = System.Random;

namespace Sources.Core {
    public class Wind {
        public float angle;
        public float lastChangeDistance;
        public float changeSpeed;
    }

    public static class WindController {
        private const int WIND_ANGLE_MAX_VALUE = 30;
        private const int WIND_CHANGE_DISTANCE = 60;
        private static bool changingAngle;
        private static float targetAngle;
        
        public static Wind Create() {
            var wind = new Wind();
            wind.changeSpeed = 15f;
            return wind;
        }
        
        public static void Update(float dt) {
            var gs = Services.Get<GameState>();
            
            if (changingAngle) {
                ChangeAngle(dt, gs);
            } else {
                if (gs.boat.transform.position.z > gs.wind.lastChangeDistance + WIND_CHANGE_DISTANCE) {
                    targetAngle = DetermineNewAngle(gs.wind.angle);
                    changingAngle = true;
                }
            }
        }
        
        private static float DetermineNewAngle(float currentAngle) {
            float result = 0;
            
            if (currentAngle == 0) {
                var sign = Services.Get<Random>().Next(2) == 0 ? -1 : 1;
                result = WIND_ANGLE_MAX_VALUE*sign;
            }
            
            return result;
        }

        private static void ChangeAngle(float dt, GameState gs) {
            if (Math.Abs(gs.wind.angle - targetAngle) < float.Epsilon) {
                gs.wind.angle = targetAngle;
                changingAngle = false;
                
                gs.wind.lastChangeDistance = gs.boat.transform.position.z;
            } else {
                gs.wind.angle = MoveTowards(gs.wind.angle, targetAngle, dt*gs.wind.changeSpeed);
            }
        }
        
        private static float MoveTowards(float current, float target, float maxDelta) {
            return Math.Abs(target - current) <= maxDelta ? target : current + Math.Sign(target - current)*maxDelta;
        }
    }
}
