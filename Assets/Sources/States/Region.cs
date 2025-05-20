using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.States {
    public enum RegionType : byte {
        Aegis,
        Styx,
        Olympia,
        Hephaestus,
        Artemis
    }

    [Serializable]
    public struct Region {
        public List<Obstacle> Obstacles;
        public Portal[] Portals;
        public RegionType Type;
    }
    
    [Serializable]
    public struct Obstacle {
        public Vec3F32 Position;
        public Vec3F32 Size;
        public int Id;
        public EntityType Type;
    }
    
    [Serializable]
    public struct Portal {
        public RegionType RegionType;
        public LaneType LaneType;
    }
}
