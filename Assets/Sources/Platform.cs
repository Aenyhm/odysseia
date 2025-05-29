using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    // Services that the platform/engine provides to the game.

    public struct GameInput {
        public float MouseDeltaX;
        public float HorizontalAxis;
        public bool MouseButtonLeftDown;
        public bool Escape;
        public bool Space;
    }

    [Serializable]
    public struct GameConf {
        public BoatConf BoatConf;
        public CoinConf CoinConf;
        public RegionConf RegionConf;
        public WindConf WindConf;
        public CannonConf CannonConf;
        public bool EnableBoatCollisions;
    }

    public struct RendererConf {
        public Dictionary<EntityType, Vec3F32> Sizes;
        public Dictionary<RegionType, List<Segment>> SegmentsByRegion;
    }
}
