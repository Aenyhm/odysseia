using System;
using System.Collections.Generic;
using Sources.Configuration;
using Sources.Mechanics;
using Sources.Toolbox;

namespace Sources.Core {
    public enum RegionType : byte { Aegis, Styx, Olympia, Hephaestus, Artemis }

    [Serializable]
    public struct Region {
        public Dictionary<EntityType, List<Entity>> ObstaclesByType;
        public SwapbackArray<SwapbackArray<Entity>> CoinLines;
        public Portal[] Portals;
        public RegionType Type;
    }

    [Serializable]
    public struct Portal {
        public RegionType RegionType;
        public LaneType LaneType;
    }
    
    public static class RegionSystem {
 
        private static SwapbackArray<RegionType> _regionTypePool = new(Enums.Count<RegionType>() - 1);

        public static void Enter(ref PlayState playState, RegionType regionType) {
            var obstaclesByType = new Dictionary<EntityType, List<Entity>> {
                { EntityType.Rock, new List<Entity>() },
                { EntityType.Trunk, new List<Entity>() },
            };
            var coinLines = new SwapbackArray<SwapbackArray<Entity>>(8);

            var availableSegments = CoreConfig.SegmentsByRegion[regionType];
            
            // On génère des tronçons de 100m d'obstacles entre 100 et 800 mètres
            for (var i = 1; i < 9; i++) {
                var segmentIndex = Rnd.Next(availableSegments.Length);
                var segment = availableSegments[segmentIndex];
                var segmentZ = i*CoreConfig.SegmentLength;
                
                GenerateObstacles(segment, segmentZ, obstaclesByType);
                if (Rnd.Next(100) <= CoreConfig.CoinSpawnPercentage) {
                    coinLines.Add(CoinMechanics.GenerateCoinLine(segmentZ));
                }
            }
            
            var newRegion = new Region {
                Type = regionType,
                ObstaclesByType = obstaclesByType,
                CoinLines = coinLines,
                Portals = GeneratePortals(regionType)
            };
            playState.Region = newRegion;
            playState.Boat.Position.Z = 0;
        }

        public static void Execute(ref PlayState playState) {
            var boat = playState.Boat;
            
            if (boat.Position.Z >= CoreConfig.PortalDistance) {
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
        
        private static void GenerateObstacles(
            SegmentConf segment, int segmentZ, Dictionary<EntityType, List<Entity>> obstaclesByType
        ) {
            var sizes = Services.Get<RendererConf>().Sizes;
            
            foreach (var obstacleInfo in segment.ObstacleConfs) {
                var obstacle = new Entity();
                obstacle.Id = EntityManager.NextId;
                
                var x = LaneMechanics.GetPosition(obstacleInfo.LaneType, CoreConfig.LaneDistance);
                if (obstacleInfo.EntityType == EntityType.Trunk) x /= 2;
                
                obstacle.Position = new Vec3F32(x, 0, segmentZ + obstacleInfo.Z);
                obstacle.Size = sizes[obstacleInfo.EntityType];
                
                obstaclesByType[obstacleInfo.EntityType].Add(obstacle);
            }
        }
        
        private static Portal[] GeneratePortals(RegionType currentRegionType) {
            var result = new Portal[CoreConfig.PortalCount];
            
            var lanePool = new SwapbackArray<LaneType>(Enums.Count<LaneType>());
            foreach (LaneType laneType in Enum.GetValues(typeof(LaneType))) {
                lanePool.Add(laneType);
            }
            
            for (var i = 0; i < result.Length; i++) {
                var portal = new Portal();
                portal.RegionType = PickRegionType(currentRegionType);
                
                var laneIndex = Rnd.Next(lanePool.Count);
                var lane = lanePool.Items[laneIndex];
                portal.LaneType = lane;
                lanePool.RemoveAt(laneIndex);
                
                result[i] = portal;
            }

            return result;
        }
        
        private static RegionType PickRegionType(RegionType currentRegionType) {
            if (_regionTypePool.Count == 0) {
                MakeRegionTypePool(currentRegionType);
            }
            
            var index = Rnd.Next(_regionTypePool.Count);
            var result = _regionTypePool.Items[index];
            _regionTypePool.RemoveAt(index);
            
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
