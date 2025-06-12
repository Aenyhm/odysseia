using Sources.Toolbox;

namespace Sources.Core {
    public static class ScoreLogic {
        // Ajout d'une valeur au score en fonction de la vitesse du bateau.
        public static void Add(GameState gameState, int value) {
            gameState.PlayState.PlayProgression.Score += (int)(value*gameState.PlayState.ScoreMultiplier);
        }
    }
    
    public static class ScoreSystem {
        public static void Execute(GameState gameState) {
            var boatSpeed = gameState.PlayState.Boat.SpeedZ;
            var minSpeed = Services.Get<GameConf>().BoatConf.SpeedZMin;
            var maxSpeed = Services.Get<GameConf>().BoatConf.SpeedMaxConf.Max;

            gameState.PlayState.ScoreMultiplier = 1 + (boatSpeed - minSpeed)/(maxSpeed - minSpeed)*2;
        }
    }
}
