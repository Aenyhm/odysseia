namespace Sources.Configuration {
    
    public struct SegmentConf {
        public ObstacleConf[] ObstacleConfs;
    }
    
    public readonly struct ObstacleConf {
        public readonly int Z;
        public readonly LaneType LaneType;
        public readonly EntityType EntityType;
        
        public ObstacleConf(EntityType entityType, LaneType laneType, int z) {
            EntityType = entityType;
            LaneType = laneType;
            Z = z;
        }
    }
}
