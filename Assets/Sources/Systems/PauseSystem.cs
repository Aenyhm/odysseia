using Sources.States;

namespace Sources.Systems {
    public class PauseSystem : AbstractSystem {
        private static bool _lastEscape;
        private static GameMode _previousMode;

        public override void Init(ref GameState gameState) {
        }
        
        public override void Update(ref GameState gameState, in GameInput input, float dt) {
            if (_lastEscape != input.Escape) {
                _lastEscape = input.Escape;
                if (input.Escape) {
                    if (gameState.GameMode == GameMode.Pause) {
                        gameState.GameMode = _previousMode;
                    } else {
                        _previousMode = gameState.GameMode;
                        gameState.GameMode = GameMode.Pause;
                    }
                }
            }
        }
    }
}
