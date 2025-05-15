namespace Sources.Core {
    public enum EntityType : byte {
        Boat,
        Rock,
        Trunk
    }

    public static class EntityManager {
        private static int _currentId;
        public static int NextId => ++_currentId;
    }
}
