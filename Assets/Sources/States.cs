using System;
using Sources.Core;

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
        public Boat Boat;
        public Region Region;
        public Wind Wind;
        public float Distance;
        public int Score;
        public int CoinCount;
        public PlayMode Mode;
    }
}
