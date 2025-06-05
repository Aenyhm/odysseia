using System;

namespace Sources.Toolbox {
    public static class Prng {
        private static readonly Random _random = new();
        
        public static int Roll(int max) => _random.Next(max);
        public static int Range(RangeI32 range) => _random.Next(range.Min, range.Max);
        public static bool Chance(int value, int max) => Roll(max) < value;
        
        // Fisher–Yates shuffle Algorithm
        public static void Shuffle<T>(T[] a) {
            for (var i = a.Length - 1; i > 0; i--) {
                var j = _random.Next(i + 1);
                (a[i], a[j]) = (a[j], a[i]);
            }
        }
    }
}
