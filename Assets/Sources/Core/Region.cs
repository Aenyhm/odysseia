using System;
using System.Collections.Generic;
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
        public SwapbackArray<Entity> Entities;
        public SwapbackArray<Coin> Coins;
        public SimpleGrid<EntityType> EntityGrid;
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
        private static readonly SwapbackArray<RegionType> _regionTypePool = new(Enums.Count<RegionType>() - 1);

        public static void Enter(ref GameState gameState, RegionType regionType) {
            ref var playState = ref gameState.PlayState;
            var gameConf = Services.Get<GameConf>();
            var regionConf = gameConf.RegionConf;
            var coinConf = gameConf.CoinConf;
            
            var rendererConf = Services.Get<RendererConf>();

            var entities = new SwapbackArray<Entity>();
            var coins = new SwapbackArray<Coin>();
            
            var entityGrid = new SimpleGrid<EntityType>(
                Enums.Count<LaneType>(), gameConf.RegionConf.RegionDistance/CoreConfig.GridScale
            );
            
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
                    
                    var offsetZ = segmentZ/CoreConfig.GridScale;
                    
                    if (Prng.Chance(spawnPct, 100)) {
                        if (entityCell.Type == EntityType.Coin) {
                            var coinLine = CoinLogic.GenerateCoinLine(in coinConf, entityCell, offsetZ);
                            coins.Append(coinLine);
                        } else {
                            var e = GenerateEntity(entityCell, offsetZ);
                            entities.Append(e);
                        }
                    }
                }
   
                segmentZ += (int)segment.Length;
            }
            
            entities.Append(RelicLogic.Create());
            
            foreach (var e in entities) {
                AddEntityToGrid(e.Type, e.Coords, ref entityGrid);
            }

            foreach (var coin in coins) {
                AddEntityToGrid(EntityType.Coin, coin.Coords, ref entityGrid);
            }

            var newRegion = new Region {
                Type = regionType,
                Entities = entities,
                EntityGrid = entityGrid,
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
        
        private static Entity GenerateEntity(EntityCell entityCell, int offsetZ) {
            var e = new Entity();
            e.Id = EntityLogic.NextId;
            e.Type = entityCell.Type;
            
            e.Coords = EntityLogic.GetAllEntityCoords(entityCell, offsetZ);
            e.Position = EntityLogic.GetPosition(e.Type, e.Coords);
            
            return e;
        }
        
        private static void AddEntityToGrid(EntityType entityType, Vec2I32[] coords, ref SimpleGrid<EntityType> entityGrid) {
            foreach (var coord in coords) {
                var gridCoords = entityGrid.CoordsToIndex(coord);
                entityGrid.Items[gridCoords] = entityType;
            }
        }
        
        private static Portal[] GeneratePortals(RegionType currentRegionType, int portalCount) {
            var result = new Portal[portalCount];
            
            var lanePool = new SwapbackArray<LaneType>(Enums.Count<LaneType>());
            foreach (LaneType laneType in Enum.GetValues(typeof(LaneType))) {
                lanePool.Append(laneType);
            }
            
            for (var i = 0; i < result.Length; i++) {
                var portal = new Portal();
                portal.RegionType = PickRegionType(currentRegionType);
                
                var laneIndex = Prng.Roll(lanePool.Count);
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
            
            var index = Prng.Roll(_regionTypePool.Count);
            var result = _regionTypePool.Items[index];
            _regionTypePool.RemoveAt(index);
            
            return result;
        }
        
        private static void MakeRegionTypePool(RegionType currentRegionType) {
            _regionTypePool.Reset();
            
            // On évite d'avoir la même région dans le portail suivant.
            foreach (RegionType rt in Enum.GetValues(typeof(RegionType))) {
                if (rt != currentRegionType) _regionTypePool.Append(rt);
            }
        }
    }
}
