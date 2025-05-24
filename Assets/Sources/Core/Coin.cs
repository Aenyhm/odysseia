using Sources.Configuration;
using Sources.Toolbox;

namespace Sources.Core {
    public static class CoinSystem {
        
        public static void Execute(ref PlayState playState) {
            var boat = playState.Boat;
            var region = playState.Region;

            for (var i = region.CoinLines.Count - 1; i >= 0; i--) {
                ref var coinLine = ref region.CoinLines.Items[i];
                
                var found = false;
                for (var j = coinLine.Count - 1; j >= 0; j--) {
                    var coin = coinLine.Items[j];
                    if (Collisions.CheckAabb(boat.Position, boat.Size, coin.Position, coin.Size)) {
                        playState.CoinCount++;
                        coinLine.RemoveAtSwapback(j);
                        playState.Score += CoreConfig.EntityScoreValues[EntityType.Coin];
                        found = true;
                        break;
                    }
                }

                if (found) {
                    if (coinLine.Count == 0) {
                        region.CoinLines.RemoveAtSwapback(i);
                        playState.Score += 10;
                    }
                    break;
                }
            }
        }
    }
}
