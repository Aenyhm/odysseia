namespace Sources.Core {
    public static class GameOverSystem {
        
        public static void Execute(ref GameState gameState) {
            if (gameState.PlayState.Mode == PlayMode.GameOver) return;
            
            if (gameState.PlayState.Boat.Health == 0) {
                gameState.PlayState.Boat.SailWindward = false;
                gameState.TotalCoinCount += gameState.PlayState.CoinCount;
                gameState.PlayState.Mode = PlayMode.GameOver;
            }
            
            // TODO: Save PlayState
        }
    }
}
