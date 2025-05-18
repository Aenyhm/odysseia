using Sources.Core;
using Sources.Toolbox;

namespace Sources.View {
    public class PortalView {
        public Vec3F32 Position;
        public RegionType RegionType;
    }

    public static class PortalViewSystem {
        private const float Y = 4f;
        
        public static PortalView Update(in Portal portal) {
            var result = new PortalView();
            result.Position = new Vec3F32(LaneHelper.GetPosition(portal.LaneType), Y, CoreConfig.PORTAL_DISTANCE);
            result.RegionType = portal.RegionType;
            return result;
        }
    }
}
