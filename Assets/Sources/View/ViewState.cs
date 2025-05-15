using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources.View {
    public struct ViewState {
        public BoatView BoatView;
        public EntityView CameraView;
        public WindView WindView;
        public RegionTheme RegionTheme;
        public List<EntityView> RockViews;
        public List<EntityView> TrunkViews;
    }
    
    public struct EntityView {
        public Vec3F32 Position;
        public Vec3F32 Rotation;
        public Vec3F32 Scale;
        public int Id;
        public EntityType Type;
    }
}
