namespace Sources.Toolbox {
    
    // Utilitaire pour accéder au temps depuis n'importe où.
    public static class Clock {
        public static float GameTime { get; private set; }
        public static float DeltaTime { get; private set; }

        public static void Update(float dt) {
            GameTime += dt;
            DeltaTime = dt;
        }
    }
}
