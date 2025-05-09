using System;
using Sources.Toolbox;

namespace Sources.Core {
    public class Wind {
        public int angle;
        public float lastChangeDistance;
    }
    
    public static class WindController {
        private const int WIND_ANGLE_MAX_VALUE = 30;
        private const int WIND_CHANGE_DISTANCE = 60;

        public static void Update() {
            var gs = Services.Get<GameState>();
            if (gs.boat.transform.position.z > gs.wind.lastChangeDistance + WIND_CHANGE_DISTANCE) {
                if (gs.wind.angle == 0) {
                    var sign = Services.Get<Random>().Next(2) == 0 ? -1 : 1;
                    gs.wind.angle = WIND_ANGLE_MAX_VALUE*sign;
                } else {
                    gs.wind.angle = 0;
                }

                gs.wind.lastChangeDistance = gs.boat.transform.position.z;
            }
        }
    }
}
