namespace Sources.Toolbox {
    public static class Clock {
        public static float Time { get; private set; }
        public static float DeltaTime { get; private set; }

        public static void Update(float dt) {
            Time += dt;
            DeltaTime = dt;
        }
    }
}
