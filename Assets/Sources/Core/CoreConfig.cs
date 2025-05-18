using System.Collections.Generic;

namespace Sources.Core {
    public static class CoreConfig {
        public const int PORTAL_DISTANCE = 1000;
        
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
