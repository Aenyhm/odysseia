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
        public Vec3F32 Position;
        public int Id;
        public int LineId;
    }
    
    public static class CoinSystem {
        private static int _currentLineId;
        private static int _currentLineLooted;

        public static void Execute(ref GameState gameState) {
            var gameConf = Services.Get<GameConf>();
            var coinConf = gameConf.CoinConf;
            
            var sizes = Services.Get<RendererConf>().Sizes;
            var coinSize = sizes[EntityType.Coin];
            var boatSize = sizes[EntityType.Boat];
            
            ref var playState = ref gameState.PlayState;
            ref var region = ref playState.Region;
            var boat = playState.Boat;
            
            var toRemoveIndices = new List<int>(coinConf.CoinLineCount);

            for (var i = 0; i < region.Coins.Count; i++) {
                var coin = region.Coins.Items[i];
                
                if (Collisions.CheckAabb(boat.Position, boatSize, coin.Position, coinSize)) {
                    playState.CoinCount++;
                    
                    playState.Score += CoreConfig.EntityScoreValues[EntityType.Coin];

                    if (coin.LineId != _currentLineId) {
                        _currentLineLooted = 0;
                        _currentLineId = coin.LineId;
                    } 
                    
                    _currentLineLooted++;
                    if (_currentLineLooted == coinConf.CoinLineCount) {
                        playState.Score += coinConf.CoinLineBonus;
                    }

                    toRemoveIndices.Add(i);
                }
            }
            
            foreach (var index in toRemoveIndices) {
                region.Coins.RemoveAtSwapback(index);
            }
        }
    }
}
