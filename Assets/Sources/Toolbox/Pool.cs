using System;
using System.Collections.Generic;

namespace Sources.Toolbox {
    public class Pool<T> {
        public readonly List<T> used = new();
        private readonly Stack<T> _free = new();
        private readonly Func<T> _create;
        
        public Pool(Func<T> create) {
            _create = create;
        }

        public T Get() {
            var item = _free.Count > 0 ? _free.Pop() : _create();
            used.Add(item);

            return item;
        }

        public void Free(T item) {
            _free.Push(item);
            used.Remove(item);
        }

        public List<T> FreeAll(Predicate<T> match) {
            var result = new List<T>();
            
            for (var i = used.Count - 1; i >= 0; i--) {
                var item = used[i];
                if (match(item)) {
                    Free(item);
                    result.Add(item);
                }
            }
            
            return result;
        }
    }
}
