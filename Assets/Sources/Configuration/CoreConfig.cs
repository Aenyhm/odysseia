using System.Collections.Generic;
using Sources.Core;

namespace Sources.Configuration {

    // TODO: Move into file
    public static class CoreConfig {
        public static readonly BoatConf BoatConf = new() {
            SpeedMaxConf = new SpeedMaxConf { DistanceStep = 500, Multiplier = 1.1f, Min = 20, Max = 40 },
            SailConf = new SailConf { AngleMax = 30f, WindwardAngleRange = 20f },
            PositionY = 0.5f,
            SpeedCollisionFactor = 0.5f,
            SpeedZStart = 10,
            SpeedZMin = 10,
            SpeedX = 30,
            HealthMax = 3,
        };
        
        public const float LaneDistance = 6f;
        public const float CoinDistance = 1.5f;
        public const int CoinLineCount = 10;
        public const int CoinSpawnPercentage = 50;

        public const int SegmentLength = 100;
        public const int PortalDistance = 1000;
        public const int PortalCount = 2;

        public static readonly WindConf WindConf = new(){ AngleMax = 30, ChangeDistance = 60 };
  
        public static readonly Dictionary<RegionType, SegmentConf[]> SegmentsByRegion = new() {
            { RegionType.Aegis, new[] {
                new SegmentConf { ObstacleConfs = new[] {
                    new ObstacleConf(EntityType.Rock, LaneType.Center, 20),
                    new ObstacleConf(EntityType.Rock, LaneType.Right, 50),
                    new ObstacleConf(EntityType.Rock, LaneType.Center, 80),
                }},
                new SegmentConf { ObstacleConfs = new[] {
                    new ObstacleConf(EntityType.Rock, LaneType.Center, 20),
                    new ObstacleConf(EntityType.Rock, LaneType.Left, 50),
                    new ObstacleConf(EntityType.Rock, LaneType.Center, 80),
                }},
                new SegmentConf { ObstacleConfs = new[] {
                    new ObstacleConf(EntityType.Trunk, LaneType.Left, 20),
                    new ObstacleConf(EntityType.Trunk, LaneType.Right, 50),
                    new ObstacleConf(EntityType.Trunk, LaneType.Left, 80),
                }},
                new SegmentConf { ObstacleConfs = new[] {
                    new ObstacleConf(EntityType.Trunk, LaneType.Right, 20),
                    new ObstacleConf(EntityType.Trunk, LaneType.Left, 50),
                    new ObstacleConf(EntityType.Trunk, LaneType.Right, 80),
                }},
                new SegmentConf { ObstacleConfs = new[] {
                    new ObstacleConf(EntityType.Trunk, LaneType.Right, 20),
                    new ObstacleConf(EntityType.Rock, LaneType.Left, 50),
                    new ObstacleConf(EntityType.Rock, LaneType.Center, 80),
                }},
                new SegmentConf { ObstacleConfs = new[] {
                    new ObstacleConf(EntityType.Trunk, LaneType.Left, 20),
                    new ObstacleConf(EntityType.Rock, LaneType.Right, 50),
                    new ObstacleConf(EntityType.Trunk, LaneType.Left, 80),
                }},
            }},
            { RegionType.Styx, new[] {
                new SegmentConf { ObstacleConfs = new ObstacleConf[] {}}
            }},
            { RegionType.Olympia, new[] {
                new SegmentConf { ObstacleConfs = new ObstacleConf[] {}}
            }},
            { RegionType.Hephaestus, new[] {
                new SegmentConf { ObstacleConfs = new ObstacleConf[] {}}
            }},
            { RegionType.Artemis, new[] {
                new SegmentConf { ObstacleConfs = new ObstacleConf[] {}}
            }},
        };
    }
}
