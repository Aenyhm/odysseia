namespace Sources.Scenes {
    public abstract class AbstractScene {
        public abstract void Init(ref GameState gameState);
        public abstract void Update(ref GameState gameState, in GameInput input);
        public abstract void Enter(ref GameState gameState);
        public abstract void Exit(ref GameState gameState);
    }
}
