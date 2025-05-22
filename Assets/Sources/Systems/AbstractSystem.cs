using Sources.States;

namespace Sources.Systems {
    public abstract class AbstractSystem {
        public abstract void Init(ref GameState gameState);
        public abstract void Update(ref GameState gameState, in GameInput input, float dt);
    }
}
