using Sources.Core;
using Sources.Toolbox;

namespace Sources.View {
    public struct BoatView {
        public Vec3F32 Position;
        public float SailAngle;
        public float SailBow;
        public float HealthRatio;
    }

    public static class BoatViewSystem {
        public static BoatView Update(in BoatView previousView, in Boat boat, in Wind wind, float dt) {
            var result = new BoatView();
            result.Position = boat.Position;
            result.SailAngle = boat.SailAngle;
            
            var targetSailBow = BoatSystem.IsSailWindward(boat.SailAngle, wind.Angle) ? 1.6f : 0.4f;
            
            result.SailBow = Maths.MoveTowards(previousView.SailBow, targetSailBow, dt);
            
            result.HealthRatio = (float)boat.Health/BoatSystem.HEALTH_MAX;
            
            return result;
        }
    }
}
