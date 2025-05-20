using System;

namespace Sources.States {
    public enum LaneType : byte {
        Left,
        Center,
        Right
    }
    
    [Serializable]
    public struct GameState {
        public Boat Boat;
        public Region Region;
        public Wind Wind;
    }
}
