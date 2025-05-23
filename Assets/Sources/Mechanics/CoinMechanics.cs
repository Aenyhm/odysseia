using Sources.Configuration;
using Sources.Toolbox;

namespace Sources.Mechanics {
    public static class CoinMechanics {
        public static SwapbackArray<Entity> GenerateCoinLine(int segmentZ) {
            var result = new SwapbackArray<Entity>(CoreConfig.CoinLineCount);
            
            var laneType = Enums.GetRandom<LaneType>();
            var x = LaneMechanics.GetPosition(laneType, CoreConfig.LaneDistance);
            var coinSize = Services.Get<RendererConf>().Sizes[EntityType.Coin];
            
            for (var i = 0; i < CoreConfig.CoinLineCount; i++) {
                var coin = new Entity();
                coin.Size = coinSize;
                coin.Position = new Vec3F32(x, 0, segmentZ + i*CoreConfig.CoinDistance);
                coin.Id = EntityManager.NextId;
                result.Add(coin);
            }
            
            return result;
        }
        
    }
}
