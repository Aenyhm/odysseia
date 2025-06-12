using System;

namespace Sources.Core {
    public static class GameOverSystem {

        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            
            if (playState.Mode == PlayMode.Play && playState.Boat.Health == 0) {
                playState.Mode = PlayMode.GameOver;
                SaveGame(gameState);
            }
        }
        
        private static void SaveGame(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            
            ref var globalProg = ref gameState.GlobalProgression;
            globalProg.CoinCount += playState.CoinCount;
            FileStorage.Save(globalProg, CoreConfig.GlobalFileName, false);
        
            var playProg = playState.PlayProgression;
            playProg.SaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            playProg.Distance = (int)playState.Boat.Distance;
            FileStorage.Save(playProg, CoreConfig.PlayFileName, true);
            
            gameState.PlayProgressions.Add(playProg);
        }
    }
}
