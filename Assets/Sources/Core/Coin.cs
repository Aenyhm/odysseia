using Sources.Toolbox;

namespace Sources.Core {
    public static class CoinSystem {
        
        public static void Execute(ref PlayState playState) {
            var boat = playState.Boat;
            var region = playState.Region;

            for (var i = region.CoinLines.Count - 1; i >= 0; i--) {
                var coinLine = region.CoinLines.Items[i];
                
                for (var j = coinLine.Count - 1; j >= 0; j--) {
                    var coin = coinLine.Items[j];
                    if (Collisions.CheckAabb(boat.Position, boat.Size, coin.Position, coin.Size)) {
                        coinLine.RemoveAt(j);
                        playState.CoinCount++;
                        playState.Score += 1;
                        break;
                    }
                }

                if (coinLine.Count == 0) {
                    region.CoinLines.RemoveAt(i);
                    playState.Score += 10;
                }
            }
        }
    }
}
