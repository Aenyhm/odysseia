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
        public const int CoinSpawnPercentage = 60;
        
        public const int EnemySpawnPercentage = 20;
        public const int MermaidEffectDistance = 40;

        public const int ZenDistance = 100;
        public const int RegionDistance = 1000;
        public const int PortalCount = 2;

        public static readonly WindConf WindConf = new(){ AngleMax = 30, ChangeDistance = 60 };
        
        public static readonly Dictionary<EntityType, int> EntityScoreValues = new () {
            { EntityType.Coin, 1 },
            { EntityType.Mermaid, 5 },
        };

        public static readonly Dictionary<RegionType, SegmentConf[]> SegmentsByRegion = new() {
            { RegionType.Aegis, new[] {
                new SegmentConf { Distance = 100, EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Rock, LaneType.Center, 20),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Right, 50),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Center, 80),
                }},
                new SegmentConf { Distance = 100, EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Rock, LaneType.Center, 20),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Left, 50),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Center, 80),
                }},
                new SegmentConf { Distance = 100, EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Left, 20),
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Right, 50),
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Left, 80),
                }},
                new SegmentConf { Distance = 100, EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Right, 20),
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Left, 50),
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Right, 80),
                }},
                new SegmentConf { Distance = 100, EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Right, 20),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Left, 50),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Center, 80),
                }},
                new SegmentConf { Distance = 100, EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Left, 20),
                    new SegmentEntityConf(EntityType.Rock, LaneType.Right, 50),
                    new SegmentEntityConf(EntityType.Trunk, LaneType.Left, 80),
                }},
            }},
            { RegionType.Styx, new[] {
                new SegmentConf { Distance = 100, EntityConfs = new SegmentEntityConf[] {}}
            }},
            { RegionType.Olympia, new[] {
                new SegmentConf { Distance = 100, EntityConfs = new SegmentEntityConf[] {}}
            }},
            { RegionType.Hephaestus, new[] {
                new SegmentConf { Distance = 100, EntityConfs = new SegmentEntityConf[] {}}
            }},
            { RegionType.Artemis, new[] {
                new SegmentConf { Distance = 100, EntityConfs = new SegmentEntityConf[] {}}
            }},
        };
    }
}
