using Sources.Core;

namespace Sources {
    // Services that the platform layer/game engine provides to the game.
    public interface IPlatform {
        void Log(string message);
        void AddEntityView(Entity entity);
        void RemoveEntityView(Entity entity);
    }
    
    // Services that the game provides to the platform layer/game engine.
    public struct GameInput {
        public float HorizontalAxis;
    }
    
    public interface IGame {
        void Update(float dt, GameInput input);
    }
}
