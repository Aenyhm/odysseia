using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    // Ne pas changer l'ordre : correspond à celui d'Unity
    public enum SceneType : byte { Title, Gameplay, Leaderboard }
    public enum PlayMode : byte { Play, Pause, GameOver }

    [Serializable]
    public struct GameState {
        public List<PlayProgression> PlayProgressions;
        public PlayState PlayState;
        public GlobalProgression GlobalProgression;
        public SceneType CurrentSceneType;
    }
    
    [Serializable]
    public struct PlayState {
        public Entities Entities;
        public Boat Boat;
        public Region Region;
        public Wind Wind;
        public Cannon Cannon;
        public PlayProgression PlayProgression;
        public int CoinCount;
        public PlayMode Mode;
    }
    
    [Serializable]
    public struct Entities {
        public SwapbackArray<Cannonball> Cannonballs;
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
