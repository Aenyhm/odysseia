using System;
using System.Collections.Generic;
using Sources.Mechanics;
using Sources.Toolbox;

namespace Sources.Core {
    public enum RegionType : byte { Aegis, Styx, Olympia, Hephaestus, Artemis }

    [Serializable]
    public struct RegionConf {
        public int ZenDistance;
        public int RegionDistance;
        public int PortalCount;
    }
    
    [Serializable]
    public struct Region {
        public List<Entity> Entities;
        public SimpleArray<Coin> Coins;
        public Portal[] Portals;
        public RegionType Type;
    }

    [Serializable]
    public struct Portal {
        public RegionType RegionType;
        public LaneType LaneType;
    }
    
    public static class RegionSystem {
 
        // -1 car on ne met pas dans le pool la région actuelle
        private static SimpleArray<RegionType> _regionTypePool = new(Enums.Count<RegionType>() - 1);

        public static void Enter(ref GameState gameState, RegionType regionType) {
            ref var playState = ref gameState.PlayState;
            var gameConf = Services.Get<GameConf>();
            var regionConf = gameConf.RegionConf;
            var coinConf = gameConf.CoinConf;
            
            var rendererConf = Services.Get<RendererConf>();

            var entities = new List<Entity>();
            var coins = new SimpleArray<Coin>(4*8*coinConf.CoinLineCount); // max 4 coin lines / segment
            
            var filledSegmentsDistance = regionConf.RegionDistance - 2*regionConf.ZenDistance;
            var availableSegments = rendererConf.SegmentsByRegion[regionType];
            var segments = GenerateSegments(availableSegments, filledSegmentsDistance);
            
            var segmentZ = regionConf.ZenDistance;
            foreach (var segment in segments) {
                foreach (var entityCell in segment.EntityCells) {
                    var spawnPct = 100;
                    if (CoreConfig.EntitySpawnPcts.TryGetValue(entityCell.Type, out var pct)) {
                        spawnPct = pct;
                    }
                    
                    if (Prng.Chance(spawnPct, 100)) {
                        if (entityCell.Type == EntityType.Coin) {
                            var coinLine = CoinMechanics.GenerateCoinLine(in coinConf, entityCell, segmentZ);
                            if (!coins.CanAdd(coinLine.Length)) coins.Resize(coins.Capacity*2);
                            coins.AddRange(coinLine);
                        } else {
                             entities.Add(GenerateEntity(entityCell, segmentZ));
                        }
                    }
                }
   
                segmentZ += (int)segment.Length;
            }

            var newRegion = new Region {
                Type = regionType,
                Entities = entities,
                Coins = coins,
                Portals = GeneratePortals(regionType, regionConf.PortalCount)
            };
            playState.Region = newRegion;
            playState.Boat.Position.Z = 0;
        }

        public static void Execute(ref GameState gameState) {
            ref var playState = ref gameState.PlayState;
            var gameConf = Services.Get<GameConf>();
            var regionConf = gameConf.RegionConf;
            
            var boat = playState.Boat;
            
            if (boat.Position.Z >= regionConf.RegionDistance) {
                var portals = playState.Region.Portals;
                var nextRegionType = playState.Region.Type;
                
                foreach (var portal in portals) {
                    if (boat.LaneType == portal.LaneType) {
                        nextRegionType = portal.RegionType;
                        break;
                    }
                }
                
                Enter(ref gameState, nextRegionType);
            }
        }
        
        // On génère des tronçons de 100m d'obstacles et 50m d'ennemis
        // en laissant une zone tranquille entre les portails.
        private static List<Segment> GenerateSegments(List<Segment> availableSegments, int filledSegmentsDistance) {
            var result = new List<Segment>();
            
            if (availableSegments.Count != 0) {
                var generatedDistance = 0;
                
                while (generatedDistance < filledSegmentsDistance - (int)SegmentLength.L50) {
                    var segmentIndex = Prng.Roll(availableSegments.Count);
                    var segment = availableSegments[segmentIndex];
         
                    result.Add(segment);
                    generatedDistance += (int)segment.Length;
                }
                
                if (generatedDistance != filledSegmentsDistance) {
                    throw new Exception(
                        $"Wrong segment distance generated: {generatedDistance}; must be {filledSegmentsDistance}."
                    );
                }
                
                Prng.Shuffle(result);
            }
            
            return result;
        }
        
        private static Entity GenerateEntity(EntityCell entityCell, int segmentZ) {
            var sizes = Services.Get<RendererConf>().Sizes;

            var e = new Entity();
            e.Id = EntityManager.NextId;
            e.Type = entityCell.Type;
            e.LaneType = (LaneType)entityCell.X; // FIXME: trunk on 2 lanes
            e.Size = sizes[entityCell.Type];

            var coords = new Vec3F32(LaneMechanics.GetSign(e.LaneType), 0, entityCell.Y);
            var dimensions = EntityDefinitions.DimensionByEntityType[entityCell.Type];
            if (dimensions.X == 2) {
                coords.X += 0.5f;
            }
            e.Position = new Vec3F32(coords.X*CoreConfig.LaneDistance, 0, segmentZ + entityCell.Y*CoreConfig.GridScale);
            
            return e;
        }
        
        private static Portal[] GeneratePortals(RegionType currentRegionType, int portalCount) {
            var result = new Portal[portalCount];
            
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
