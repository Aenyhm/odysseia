using System.Collections.Generic;

namespace Sources.States {
    public readonly struct ObstacleInfo {
        public readonly int Z;
        public readonly LaneType LaneType;
        public readonly EntityType EntityType;
        
        public ObstacleInfo(EntityType entityType, LaneType laneType, int z) {
            EntityType = entityType;
            LaneType = laneType;
            Z = z;
        }
    }
    
    public struct SegmentInfo {
        public ObstacleInfo[] ObstacleInfos;
    }
    

    public static class CoreConfig {
        public const float LaneDistance = 6f;
        public const int SegmentLength = 100;
        public const int PortalDistance = 1000;
        public const int PortalCount = 2;
        
        public static readonly SailConf SailConf = new(){ AngleMax = 30f, WindwardAngleRange = 20f };
        public static readonly SpeedMaxConf SpeedMaxConf = new(){ DistanceStep = 500, Multiplier = 1.1f, Min = 20, Max = 40 };
        public static readonly WindConf WindConf = new(){ AngleMax = 30, ChangeDistance = 60 };
  
        public static readonly Dictionary<RegionType, SegmentInfo[]> SegmentsByRegion = new() {
            { RegionType.Aegis, new[] {
                new SegmentInfo { ObstacleInfos = new[] {
                    new ObstacleInfo(EntityType.Rock, LaneType.Center, 20),
                    new ObstacleInfo(EntityType.Rock, LaneType.Right, 40),
                    new ObstacleInfo(EntityType.Rock, LaneType.Center, 60),
                    new ObstacleInfo(EntityType.Rock, LaneType.Left, 80),
                }},
                new SegmentInfo { ObstacleInfos = new[] {
                    new ObstacleInfo(EntityType.Trunk, LaneType.Left, 30),
                    new ObstacleInfo(EntityType.Trunk, LaneType.Right, 50),
                    new ObstacleInfo(EntityType.Trunk, LaneType.Left, 80),
                }},
            }},
            { RegionType.Styx, new[] {
                new SegmentInfo { ObstacleInfos = new ObstacleInfo[] {}}
            }},
            { RegionType.Olympia, new[] {
                new SegmentInfo { ObstacleInfos = new ObstacleInfo[] {}}
            }},
            { RegionType.Hephaestus, new[] {
                new SegmentInfo { ObstacleInfos = new ObstacleInfo[] {}}
            }},
            { RegionType.Artemis, new[] {
                new SegmentInfo { ObstacleInfos = new ObstacleInfo[] {}}
            }},
        };
    }
}
