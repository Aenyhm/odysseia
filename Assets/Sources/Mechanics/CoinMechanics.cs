using Sources.Configuration;
using Sources.Core;
using Sources.Toolbox;

namespace Sources.Mechanics {
    public static class CoinMechanics {
        public static SimpleArray<Entity> GenerateCoinLine(int segmentZ) {
            var result = new SimpleArray<Entity>(CoreConfig.CoinLineCount);
            
            var laneType = Enums.GetRandom<LaneType>();
            var x = LaneMechanics.GetPosition(laneType, CoreConfig.LaneDistance);
            var coinSize = Services.Get<RendererConf>().Sizes[EntityType.Coin];
            
            for (var i = 0; i < CoreConfig.CoinLineCount; i++) {
                var coin = new Entity();
                coin.Size = coinSize;
                coin.Position = new Vec3F32(x, 0, segmentZ + i*CoreConfig.CoinDistance);
                coin.Id = EntityManager.NextId;
                coin.Type = EntityType.Coin;
                result.Add(coin);
            }
            
            return result;
        }
        
    }
}
