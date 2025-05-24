using System;
using System.Diagnostics.Contracts;

namespace Sources.Toolbox {
    public static class Enums {

        [Pure]
        public static int Count<T>() where T : Enum {
            return Enum.GetValues(typeof(T)).Length;
        }

        public static T GetRandom<T>() where T : Enum {
            var count = Count<T>();
            var index = Prng.Roll(count);
            return (T)Enum.ToObject(typeof(T), index);
        }
    }
}
