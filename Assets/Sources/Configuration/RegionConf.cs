using Sources.Core;

namespace Sources.Configuration {
    
    public struct SegmentConf {
        public SegmentEntityConf[] EntityConfs;
        public int Distance; // 50 ou 100
    }
    
    public readonly struct SegmentEntityConf {
        public readonly int Z;
        public readonly LaneType LaneType;
        public readonly EntityType EntityType;
        
        public SegmentEntityConf(EntityType entityType, LaneType laneType, int z) {
            EntityType = entityType;
            LaneType = laneType;
            Z = z;
        }
    }
}
