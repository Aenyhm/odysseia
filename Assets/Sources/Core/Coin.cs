using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct CoinConf {
        public float CoinDistance;
        public int CoinLineCount;
        public int CoinLineBonus;
    }
    
    [Serializable]
    public struct Coin {
        public Vec2I32[] Coords;
        public Vec3F32 Position;
        public int Id;
        public int LineId;
    }
    
    public static class CoinLogic {
        private static int _currentLineId;
        
        public static Coin[] GenerateCoinLine(in CoinConf coinConf, EntityCell entityCell, int offsetZ) {
            var result = new Coin[coinConf.CoinLineCount];
            
            var x = LaneLogic.GetPosition(entityCell.X);
            var lineId = ++_currentLineId;
            
            for (var i = 0; i < coinConf.CoinLineCount; i++) {
                var pos = new Vec3F32(x, 0, (offsetZ + entityCell.Y)*CoreConfig.GridScale + i*coinConf.CoinDistance);
                
                var coin = new Coin();
                coin.Coords = EntityLogic.GetAllEntityCoords(entityCell, offsetZ);
                coin.Position = pos;
                coin.Id = EntityLogic.NextId;
                coin.LineId = lineId;
                result[i] = coin;
            }
            
            return result;
        }
    }
    
    public static class CoinSystem {
        private static int _currentLineId;
        private static int _currentLineLooted;

        public static void Execute(GameState gameState) {
            var gameConf = Services.Get<GameConf>();
            var coinConf = gameConf.CoinConf;
            
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;
            var coinBox = boxes[EntityType.Coin];
            var boatBox = boxes[EntityType.Boat];
            
            ref var playState = ref gameState.PlayState;
            ref var region = ref playState.Region;
            var boat = playState.Boat;
            
            var toRemoveIndices = new List<int>(coinConf.CoinLineCount);

            for (var i = 0; i < region.Coins.Count; i++) {
                var coin = region.Coins.Items[i];
                
                if (Collisions.CheckCollisionBoxes(boat.Position, boatBox, coin.Position, coinBox)) {
                    playState.CoinCount++;
                    
                    ScoreLogic.Add(gameState, EntityConf.EntityScoreValues[EntityType.Coin]);

                    if (coin.LineId != _currentLineId) {
                        _currentLineLooted = 0;
                        _currentLineId = coin.LineId;
                    } 
                    
                    _currentLineLooted++;
                    if (_currentLineLooted == coinConf.CoinLineCount) {
                        ScoreLogic.Add(gameState, coinConf.CoinLineBonus);
                    }

                    toRemoveIndices.Add(i);
                }
            }
            
            foreach (var index in toRemoveIndices) {
                region.Coins.RemoveAt(index);
            }
        }
    }
}
