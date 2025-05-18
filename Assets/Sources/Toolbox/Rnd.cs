using System;

namespace Sources.Toolbox {
    public static class Rnd {
        private static readonly Random _random = new();
        
        public static int Next(int min, int max) => _random.Next(min, max);
        public static int Next(int max) => Next(0, max);
    }
}
