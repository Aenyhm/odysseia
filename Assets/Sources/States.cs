using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    // Ne pas changer l'ordre : correspond à celui d'Unity
    public enum SceneType : byte { Title, Gameplay, Leaderboard }
    public enum PlayMode : byte { Play, Pause, GameOver }

    [Serializable]
    public class GameState {
        public List<PlayProgression> PlayProgressions;
        public PlayState PlayState;
        public GameInput Input;
        public GlobalProgression GlobalProgression;
        public SceneType CurrentSceneType;
    }
    
    [Serializable]
    public struct PlayState {
        public SwapbackArray<Cannonball> Ammos;
        public SwapbackArray<Cannonball> Cannonballs;
        public Boat Boat;
        public Region Region;
        public Wind Wind;
        public Cannon Cannon;
        public PlayProgression PlayProgression;
        public float ScoreMultiplier;
        public int CoinCount;
        public PlayMode Mode;
    }

    [Serializable]
    public struct GlobalProgression {
        public int CoinCount;
    }
    
    [Serializable]
    public struct PlayProgression {
        public string SaveTime;
        public int Distance;
        public int Score;
    }
}
