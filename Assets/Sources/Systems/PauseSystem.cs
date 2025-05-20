using Sources.States;

namespace Sources.Systems {
    public static class PauseSystem {
        private static bool _lastEscape;

        public static void Update(ref GameState gameState, in GameInput input) {
            if (_lastEscape != input.Escape) {
                _lastEscape = input.Escape;
                if (input.Escape) {
                    gameState.Pause = !gameState.Pause;
                }
            }
        }
    }
}
