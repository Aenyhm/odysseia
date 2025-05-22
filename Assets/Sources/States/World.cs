using System;

namespace Sources.States {
    public enum LaneType : byte {
        Left,
        Center,
        Right
    }
    
    public enum SceneType : byte {
        Title,
        Gameplay,
    }
    
    public enum GameMode : byte {
        Pause,
        Run,
        GameOver,
    }
    
    [Serializable]
    public struct GameState {
        public Boat Boat;
        public Region Region;
        public Wind Wind;
        public int RunCoinCount;
        public int TotalCoinCount;
        public SceneType CurrentSceneType;
        public GameMode GameMode;
    }
}
