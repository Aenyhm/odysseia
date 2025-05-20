using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources {
    // Services that the platform/engine provides to the game.
    
    public struct GameInput {
        public float MouseDeltaX;
        public float HorizontalAxis;
        public bool MouseButtonLeftDown;
        public bool Escape;
    }
    
    public struct RendererConf {
        public Dictionary<EntityType, Vec3F32> Sizes;
    }
}
