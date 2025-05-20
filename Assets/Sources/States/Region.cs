using System;
using System.Collections.Generic;

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
        public Dictionary<EntityType, List<Entity>> ObstaclesByType;
        public List<Entity> Coins;
        public Portal[] Portals;
        public RegionType Type;
    }

    [Serializable]
    public struct Portal {
        public RegionType RegionType;
        public LaneType LaneType;
    }
}
