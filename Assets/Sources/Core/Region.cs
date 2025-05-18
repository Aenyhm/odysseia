using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    public enum RegionType : byte {
        Aegis,
        Styx,
        Olympia,
        Hephaestus,
        Artemis
    }
    
    public struct Portal {
        public RegionType RegionType;
        public LaneType LaneType;
    }
    
    public struct Region {
        public List<Obstacle> Obstacles;
        public Portal Portal;
        public RegionType Type;
    }
    
    public static class RegionSystem {
        private const int SEGMENT_LENGTH = 100;
        private const RegionType START_REGION_TYPE = RegionType.Aegis;
        
        private static SimpleArray<RegionType> _regionTypePool = new(Enums.Count<RegionType>() - 1, false);
        
        public static void Init(ref CoreState coreState) {
            Enter(ref coreState, START_REGION_TYPE);
        }
        
        public static void Update(ref CoreState coreState) {
            var boat = coreState.Boat;
            var portal = coreState.Region.Portal;
            
            if (boat.Position.Z >= CoreConfig.PORTAL_DISTANCE) {
                var regionType = coreState.Region.Type;
                if (boat.LaneType == portal.LaneType) regionType = portal.RegionType;
                Enter(ref coreState, regionType);
            }
        }
        
        private static void Enter(ref CoreState coreState, RegionType regionType) {
            var obstacles = new List<Obstacle>();
            
            var availableSegments = CoreConfig.SegmentsByRegion[regionType];
            
            // On génère des tronçons d'obstacles entre 100 et 800 mètres
            for (var i = 1; i < 9; i++) {
                var segmentIndex = Rnd.Next(availableSegments.Length);
                var segment = availableSegments[segmentIndex];
                var newObstacles = ObstacleSystem.GenerateObstacles(segment, i*SEGMENT_LENGTH);
                obstacles.AddRange(newObstacles);
            }
            
            var newRegion = new Region {
                Type = regionType,
                Obstacles = obstacles,
                Portal = GeneratePortal(regionType)
            };
            coreState.Region = newRegion;
            coreState.Boat.Position.Z = 0;
        }
                
        private static Portal GeneratePortal(RegionType currentRegionType) {
            var result = new Portal();
            result.RegionType = PickRegionType(currentRegionType);
            result.LaneType = Enums.GetRandom<LaneType>();
            return result;
        }
        
        private static RegionType PickRegionType(RegionType currentRegionType) {
            if (_regionTypePool.Count == 0) {
                _makeRegionTypePool(currentRegionType);
            }
            
            var index = Rnd.Next(_regionTypePool.Count);
            var result = _regionTypePool.Items[index];
            _regionTypePool.RemoveAt(index);
            
            return result;
        }
        
        private static void _makeRegionTypePool(RegionType currentRegionType) {
            _regionTypePool.Reset();
            
            // On évite d'avoir la même région dans le portail suivant.
            foreach (RegionType rt in Enum.GetValues(typeof(RegionType))) {
                if (rt != currentRegionType) _regionTypePool.Add(rt);
            }
        }
    }
}
