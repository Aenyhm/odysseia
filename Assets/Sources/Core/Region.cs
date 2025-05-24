using System;
using System.Collections.Generic;
using Sources.Configuration;
using Sources.Mechanics;
using Sources.Toolbox;

namespace Sources.Core {
    public enum RegionType : byte { Aegis, Styx, Olympia, Hephaestus, Artemis }

    [Serializable]
    public struct Region {
        public List<Entity> Entities;
        public SimpleArray<SimpleArray<Entity>> CoinLines;
        public Portal[] Portals;
        public RegionType Type;
    }

    [Serializable]
    public struct Portal {
        public RegionType RegionType;
        public LaneType LaneType;
    }
    
    public static class RegionSystem {
 
        // TODO: explain -1
        private static SimpleArray<RegionType> _regionTypePool = new(Enums.Count<RegionType>() - 1);

        public static void Enter(ref PlayState playState, RegionType regionType) {
            var segmentConfs = GenerateSegmentConfs(regionType);
            var entities = new List<Entity>();
            var coinLines = new SimpleArray<SimpleArray<Entity>>(8);
            
            var segmentZ = CoreConfig.ZenDistance;
            foreach (var segmentConf in segmentConfs) {
                entities.AddRange(GenerateSegmentEntities(segmentConf, segmentZ));
                
                if (Prng.Chance(CoreConfig.CoinSpawnPercentage, 100)) {
                    coinLines.Add(CoinMechanics.GenerateCoinLine(segmentZ));
                }
                
                segmentZ += segmentConf.Distance;
            }

            var newRegion = new Region {
                Type = regionType,
                Entities = entities,
                CoinLines = coinLines,
                Portals = GeneratePortals(regionType)
            };
            playState.Region = newRegion;
            playState.Boat.Position.Z = 0;
        }

        public static void Execute(ref PlayState playState) {
            var boat = playState.Boat;
            
            if (boat.Position.Z >= CoreConfig.RegionDistance) {
                var portals = playState.Region.Portals;
                var nextRegionType = playState.Region.Type;
                
                foreach (var portal in portals) {
                    if (boat.LaneType == portal.LaneType) {
                        nextRegionType = portal.RegionType;
                        break;
                    }
                }
                
                Enter(ref playState, nextRegionType);
            }
        }
        
        // On génère des tronçons de 100m d'obstacles et 50m d'ennemis
        // en laissant une zone tranquille entre les portails.
        private static List<SegmentConf> GenerateSegmentConfs(RegionType regionType) {
            var result = new List<SegmentConf>();
            
            var availableSegments = CoreConfig.SegmentsByRegion[regionType];
            
            var generatedDistance = 0;
            
            const int filledSegmentsDistance = CoreConfig.RegionDistance - 2*CoreConfig.ZenDistance;
            
            while (generatedDistance < filledSegmentsDistance) {
                SegmentConf segmentConf;
                if (
                    generatedDistance == filledSegmentsDistance - 50 ||
                    Prng.Chance(CoreConfig.EnemySpawnPercentage, 100)
                ) {
                    segmentConf = GenerateEnemySegmentConf();
                } else {
                    var segmentIndex = Prng.Roll(availableSegments.Length);
                    segmentConf = availableSegments[segmentIndex];
                }
                
                result.Add(segmentConf);
                generatedDistance += segmentConf.Distance;
            }
            
            if (generatedDistance != filledSegmentsDistance) {
                throw new Exception($"Wrong segment distance generated: {generatedDistance}; must be {filledSegmentsDistance}.");
            }
            Prng.Shuffle(result);

            return result;
        }
        
        private static SegmentConf GenerateEnemySegmentConf() {
            var laneType = Enums.GetRandom<LaneType>();
            
            return new SegmentConf {
                Distance = 50,
                EntityConfs = new[] {
                    new SegmentEntityConf(EntityType.Mermaid, laneType, CoreConfig.MermaidEffectDistance),
                },
            };
        }

        private static List<Entity> GenerateSegmentEntities(SegmentConf segmentConf, int segmentZ) {
            var result = new List<Entity>();
            
            var sizes = Services.Get<RendererConf>().Sizes;
            
            foreach (var obstacleInfo in segmentConf.EntityConfs) {
                var e = new Entity();
                e.Id = EntityManager.NextId;
                
                var x = LaneMechanics.GetPosition(obstacleInfo.LaneType, CoreConfig.LaneDistance);
                if (obstacleInfo.EntityType == EntityType.Trunk) x /= 2;
                
                e.Position = new Vec3F32(x, 0, segmentZ + obstacleInfo.Z);
                e.Size = sizes[obstacleInfo.EntityType];
                e.Type = obstacleInfo.EntityType;
                e.LaneType = obstacleInfo.LaneType;

                result.Add(e);
            }
            
            return result;
        }
        
        private static Portal[] GeneratePortals(RegionType currentRegionType) {
            var result = new Portal[CoreConfig.PortalCount];
            
            var lanePool = new SimpleArray<LaneType>(Enums.Count<LaneType>());
            foreach (LaneType laneType in Enum.GetValues(typeof(LaneType))) {
                lanePool.Add(laneType);
            }
            
            for (var i = 0; i < result.Length; i++) {
                var portal = new Portal();
                portal.RegionType = PickRegionType(currentRegionType);
                
                var laneIndex = Prng.Roll(lanePool.Count);
                var lane = lanePool.Items[laneIndex];
                portal.LaneType = lane;
                lanePool.RemoveAtSwapback(laneIndex);
                
                result[i] = portal;
            }

            return result;
        }
        
        private static RegionType PickRegionType(RegionType currentRegionType) {
            if (_regionTypePool.Count == 0) {
                MakeRegionTypePool(currentRegionType);
            }
            
            var index = Prng.Roll(_regionTypePool.Count);
            var result = _regionTypePool.Items[index];
            _regionTypePool.RemoveAtSwapback(index);
            
            return result;
        }
        
        private static void MakeRegionTypePool(RegionType currentRegionType) {
            _regionTypePool.Reset();
            
            // On évite d'avoir la même région dans le portail suivant.
            foreach (RegionType rt in Enum.GetValues(typeof(RegionType))) {
                if (rt != currentRegionType) _regionTypePool.Add(rt);
            }
        }
    }
}
