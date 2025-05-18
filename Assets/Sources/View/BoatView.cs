using Sources.Core;
using Sources.Toolbox;

namespace Sources.View {
    public struct BoatView {
        public Vec3F32 Position;
        public float Distance;
        public float SailAngle;
        public float SailBow;
        public float HealthRatio;
    }

    public static class BoatViewSystem {
        private const float MIN_BOW = 0.4f;
        private const float MAX_BOW = 0.6f;
        
        public static BoatView Update(in BoatView previousView, in Boat boat, in Wind wind, float dt) {
            var result = new BoatView();
            result.Position = boat.Position;
            result.Distance = boat.Distance;
            result.SailAngle = boat.SailAngle;
            result.HealthRatio = (float)boat.Health/BoatSystem.HEALTH_MAX;
            
            var targetSailBow = BoatSystem.IsSailWindward(boat.SailAngle, wind.Angle) ? MAX_BOW : MIN_BOW;
            result.SailBow = Maths.MoveTowards(previousView.SailBow, targetSailBow, dt);
            
            return result;
        }
    }
}
