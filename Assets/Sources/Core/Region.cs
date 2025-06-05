using System;
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

        private static SwapbackArray<RegionType> _regionTypePool;
        
        public static void Init(GameState gameState) {
            ref var region = ref gameState.PlayState.Region;
            var regionConf = Services.Get<GameConf>().RegionConf;
            
            // -1 car on ne met pas dans le pool la région actuelle
            _regionTypePool = new SwapbackArray<RegionType>(Enums.Count<RegionType>() - 1);
            
            region.Entities = new SwapbackArray<Entity>();
            region.Coins = new SwapbackArray<Coin>();
            region.EntityGrid = new SimpleGrid<EntityType>(
                Enums.Count<LaneType>(),
                regionConf.RegionDistance/CoreConfig.GridScale
            );
            
            Enter(gameState, RegionType.Aegis);
        }

        private static void Enter(GameState gameState, RegionType regionType) {
            ref var playState = ref gameState.PlayState;
            ref var region = ref gameState.PlayState.Region;
            var gameConf = Services.Get<GameConf>();
            var regionConf = gameConf.RegionConf;
            var coinConf = gameConf.CoinConf;
            var rendererConf = Services.Get<RendererConf>();

            region.Entities.Reset();
            region.Coins.Reset();
            region.EntityGrid.Reset();
            
            var filledSegmentsDistance = regionConf.RegionDistance - 2*regionConf.ZenDistance;
            var availableSegments = rendererConf.SegmentsByRegion[regionType];
            var segments = SegmentLogic.GenerateSegments(availableSegments, filledSegmentsDistance);
            
            var segmentZ = regionConf.ZenDistance;
            foreach (var segment in segments) {
                foreach (var entityCell in segment.EntityCells) {
                    var spawnPct = 100;
                    if (EntityConf.EntitySpawnPcts.TryGetValue(entityCell.Type, out var pct)) {
                        spawnPct = pct;
                    }
                    
                    var offsetZ = segmentZ/CoreConfig.GridScale;
                    
                    if (Prng.Chance(spawnPct, 100)) {
                        if (entityCell.Type == EntityType.Coin) {
                            var coinLine = CoinLogic.GenerateCoinLine(in coinConf, entityCell, offsetZ);
                            region.Coins.Append(coinLine);
                        } else {
                            var e = EntityLogic.GenerateEntity(entityCell, offsetZ);
                            region.Entities.Append(e);
                        }
                    }
                }
   
                segmentZ += (int)segment.Length;
            }
            
            region.Entities.Append(RelicLogic.Create());
            
            foreach (var e in region.Entities) {
                AddEntityToGrid(e.Type, e.Coords, ref region.EntityGrid);
            }

            foreach (var coin in region.Coins) {
                AddEntityToGrid(EntityType.Coin, coin.Coords, ref region.EntityGrid);
            }
            
            region.Type = regionType;
            region.Portals = GeneratePortals(regionType, regionConf.PortalCount);

            playState.Boat.Position.Z = 0;
        }

        public static void Execute(GameState gameState) {
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
                
                Enter(gameState, nextRegionType);
            }
        }

        private static void AddEntityToGrid(EntityType entityType, Vec2I32[] coords, ref SimpleGrid<EntityType> entityGrid) {
            foreach (var coord in coords) {
                var gridCoords = entityGrid.CoordsToIndex(coord);
                entityGrid.Items[gridCoords] = entityType;
            }
        }
        
        private static Portal[] GeneratePortals(RegionType currentRegionType, int portalCount) {
            var result = new Portal[portalCount];
            
            var laneTypes = Enums.Members<LaneType>();
            Prng.Shuffle(laneTypes);
            
            for (var i = 0; i < result.Length; i++) {
                Portal portal;
                portal.RegionType = PickRegionType(currentRegionType);
                portal.LaneType = laneTypes[i];
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
