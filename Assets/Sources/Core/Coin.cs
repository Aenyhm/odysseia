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
    public struct CoinData {
        public int LineId;
    }
    
    public static class CoinLogic {
        private static int _currentLineId;
        
        public static Entity[] GenerateCoinLine(in CoinConf coinConf, EntityCell entityCell, int offsetZ) {
            var result = new Entity[coinConf.CoinLineCount];
            
            var x = LaneLogic.GetPosition(entityCell.X);
            var lineId = ++_currentLineId;
            
            for (var i = 0; i < coinConf.CoinLineCount; i++) {
                var pos = new Vec3F32(x, 0, (offsetZ + entityCell.Y)*CoreConfig.GridScale + i*coinConf.CoinDistance);
                
                var coin = new Entity();
                coin.Type = EntityType.Coin;
                coin.Coords = EntityLogic.GetAllEntityCoords(entityCell, offsetZ);
                coin.Position = pos;
                coin.Id = EntityLogic.NextId;
                coin.CoinData.LineId = lineId;
                result[i] = coin;
            }
            
            return result;
        }
    }
    
    public static class CoinSystem {
        private static int _currentLineId;
        private static int _currentLineLooted;
        
        public static Action LineCompleted;

        public static void Execute(GameState gameState) {
            var gameConf = Services.Get<GameConf>();
            var coinConf = gameConf.CoinConf;
            
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;
            var coinBox = boxes[EntityType.Coin];
            var boatBox = boxes[EntityType.Boat];
            
            ref var playState = ref gameState.PlayState;
            var coins = playState.Region.EntitiesByType[EntityType.Coin];
            var boat = playState.Boat;
            
            for (var i = 0; i < coins.Count; i++) {
                ref var e = ref coins.Items[i];
                
                if (Collisions.CheckCollisionBoxes(boat.Position, boatBox, e.Position, coinBox)) {
                    playState.CoinCount++;
                    ScoreLogic.Add(gameState, EntityConf.EntityScoreValues[EntityType.Coin]);

                    if (e.CoinData.LineId != _currentLineId) {
                        _currentLineLooted = 0;
                        _currentLineId = e.CoinData.LineId;
                    } 
                    
                    _currentLineLooted++;
                    if (_currentLineLooted == coinConf.CoinLineCount) {
                        LineCompleted.Invoke();
                        ScoreLogic.Add(gameState, coinConf.CoinLineBonus);
                    }

                    e.Destroy = true;
                }
            }
        }
    }
}
