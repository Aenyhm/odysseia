using System.Collections.Generic;
using Sources.States;
using Sources.Toolbox;

namespace Sources {
    public static class Blueprints {

        public static Boat CreateBoat(Vec3F32 size) {
            var boat = new Boat();
            boat.CollisionIds = new HashSet<int>();
            boat.SailConf = CoreConfig.SailConf;
            boat.SpeedMaxConf = CoreConfig.SpeedMaxConf;
            boat.Position.Y = 0.5f;
            boat.Size = size;
            boat.SailAngle = 0f;
            boat.SpeedCollisionFactor = 0.5f;
            boat.SpeedZ = CoreConfig.BoatSpeedZStart;
            boat.SpeedZMin = 10;
            boat.SpeedX = 30;
            boat.Health = new Health { Max = 3, Value = 3 };
            boat.LaneType = LaneType.Center;
            
            return boat;
        }
        
        public static Wind CreateWind() {
            var wind = new Wind();
            wind.Conf = CoreConfig.WindConf;
            
            return wind;
        }
    }
}
