namespace Sources.Core {
    public static class PauseSystem {
        private static PlayMode _previousMode;

        public static void Execute(ref PlayState playState, in GameInput input) {
            if (input.Escape) {
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
