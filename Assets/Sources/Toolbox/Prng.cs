using System;
using System.Collections.Generic;

namespace Sources.Toolbox {
    public static class Prng {
        private static readonly Random _random = new();
        
        public static int Roll(int max) => _random.Next(max);
        public static bool Chance(int value, int max) => Roll(max) < value;
        
        // Fisher–Yates shuffle Algorithm
        public static void Shuffle<T>(List<T> l) {
            for (var i = l.Count - 1; i > 0; i--) {
                var j = _random.Next(i + 1);
                (l[i], l[j]) = (l[j], l[i]);
            }
        }
    }
}
