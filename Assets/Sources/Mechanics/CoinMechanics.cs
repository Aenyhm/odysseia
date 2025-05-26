using Sources.Core;
using Sources.Toolbox;

namespace Sources.Mechanics {
    public static class CoinMechanics {
        private static int _currentLineId;
        
        public static Coin[] GenerateCoinLine(in CoinConf coinConf, EntityCell entityCell, int segmentZ) {
            var result = new Coin[coinConf.CoinLineCount];
            
            var laneType = (LaneType)entityCell.X;
            var x = LaneMechanics.GetPosition(laneType, CoreConfig.LaneDistance);
            var lineId = ++_currentLineId;
            
            for (var i = 0; i < coinConf.CoinLineCount; i++) {
                var pos = new Vec3F32(x, 0, segmentZ + entityCell.Y*CoreConfig.GridScale + i*coinConf.CoinDistance);
                
                var coin = new Coin();
                coin.Position = pos;
                coin.Id = EntityManager.NextId;
                coin.LineId = lineId;
                result[i] = coin;
            }
            
            return result;
        }
    }
}
