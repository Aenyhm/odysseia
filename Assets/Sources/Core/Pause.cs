namespace Sources.Core {
    public static class PauseSystem {
        private static PlayMode _previousMode;

        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            
            if (gameState.Input.Escape) {
                if (playState.Mode == PlayMode.Pause) {
                    playState.Mode = _previousMode;
                } else {
                    _previousMode = playState.Mode;
                    playState.Mode = PlayMode.Pause;
                }
            }
        }
    }
}
