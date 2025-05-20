using System;
using System.Collections.Generic;
using Sources.Mechanics;
using Sources.States;
using Sources.Toolbox;

namespace Sources.Systems {
    public static class RegionSystem {
        private static SimpleArray<RegionType> _regionTypePool = new(Enums.Count<RegionType>() - 1, false);
        private static RendererConf _rendererConf;

        public static void Init(in RendererConf rendererConf, ref GameState gameState) {
            _rendererConf = rendererConf;

            Enter(ref gameState, RegionType.Aegis);
        }
        
        public static void Update(ref GameState gameState) {
            var boat = gameState.Boat;
            
            if (boat.Position.Z >= CoreConfig.PortalDistance) {
                var portals = gameState.Region.Portals;
                var nextRegionType = gameState.Region.Type;
                
                foreach (var portal in portals) {
                    if (boat.LaneType == portal.LaneType) {
                        nextRegionType = portal.RegionType;
                        break;
                    }
                }
                
                Enter(ref gameState, nextRegionType);
            }
        }
        
        private static void Enter(ref GameState gameState, RegionType regionType) {
            var obstacles = new List<Obstacle>();
            
            var availableSegments = CoreConfig.SegmentsByRegion[regionType];
            
            // On génère des tronçons d'obstacles entre 100 et 800 mètres
            for (var i = 1; i < 9; i++) {
                var segmentIndex = Rnd.Next(availableSegments.Length);
                var segment = availableSegments[segmentIndex];
                var newObstacles = GenerateObstacles(segment, i*CoreConfig.SegmentLength);
                obstacles.AddRange(newObstacles);
            }
            
            var newRegion = new Region {
                Type = regionType,
                Obstacles = obstacles,
                Portals = GeneratePortals(regionType)
            };
            gameState.Region = newRegion;
            gameState.Boat.Position.Z = 0;
        }
        
        private static List<Obstacle> GenerateObstacles(SegmentInfo segment, int segmentZ) {
            var result = new List<Obstacle>();
            
            foreach (var obstacleInfo in segment.ObstacleInfos) {
                var obstacle = new Obstacle();
                obstacle.Type = obstacleInfo.EntityType;
                obstacle.Id = EntityManager.NextId;
                
                var x = LaneMechanics.GetPosition(obstacleInfo.LaneType, CoreConfig.LaneDistance);
                if (obstacleInfo.EntityType == EntityType.Trunk) x /= 2;
                
                obstacle.Position = new Vec3F32(x, 0, segmentZ + obstacleInfo.Z);
                obstacle.Size = _rendererConf.Sizes[obstacleInfo.EntityType];
                
                result.Add(obstacle);
            }
            
            return result;
        }
                
        private static Portal[] GeneratePortals(RegionType currentRegionType) {
            var result = new Portal[CoreConfig.PortalCount];
            
            var lanePool = new SimpleArray<LaneType>(Enums.Count<LaneType>(), false);
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
