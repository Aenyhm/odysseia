using Sources.Core;

namespace Sources {
    // Services that the platform layer/game engine provides to the game.
    public interface IPlatform {
        void Log(string message);
    }
    
    // Services that the game provides to the platform layer/game engine.
    public struct GameInput {
        public bool MouseButtonLeftDown;
        public float MouseDeltaX;
        public float HorizontalAxis;
    }
}
