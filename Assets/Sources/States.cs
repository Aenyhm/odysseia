using System;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    public enum SceneType : byte { Title, Gameplay }
    public enum PlayMode : byte { Play, Pause, GameOver }

    [Serializable]
    public struct GameState {
        public PlayState PlayState;
        public int TotalCoinCount;
        public SceneType CurrentSceneType;
    }
    
    [Serializable]
    public struct PlayState {
        public Entities Entities;
        public Boat Boat;
        public Region Region;
        public Wind Wind;
        public Cannon Cannon;
        public float Distance;
        public int Score;
        public int CoinCount;
        public PlayMode Mode;
    }
    
    [Serializable]
    public struct Entities {
        public SwapbackArray<Cannonball> Cannonballs;
    }
}
