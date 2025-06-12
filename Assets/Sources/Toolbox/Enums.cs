using System;
using System.Diagnostics.Contracts;

namespace Sources.Toolbox {
    
    public static class Enums {
        
        [Pure]
        public static T[] Members<T>() where T : Enum {
            return (T[])Enum.GetValues(typeof(T));
        }

        [Pure]
        public static int Count<T>() where T : Enum {
            return Members<T>().Length;
        }

        public static T GetRandom<T>() where T : Enum {
            var count = Count<T>();
            var index = Prng.Roll(count);
            return (T)Enum.ToObject(typeof(T), index);
        }
    }
}
