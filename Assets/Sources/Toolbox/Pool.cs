using System;
using System.Collections.Generic;

namespace Sources.Toolbox {
    
    // Object Pool: permet de réutiliser des instances de classes plutôt que
    // de les détruire et de les recréer (et donc d'allouer de la mémoire).
    public class Pool<T> where T : class {
        private readonly Stack<T> _free = new();
        private readonly Func<T> _createFn;
        
        public Pool(Func<T> createFn) {
            _createFn = createFn;
        }

        public T Get() {
            var item = _free.Count > 0 ? _free.Pop() : _createFn();
            return item;
        }

        public void Free(T item) {
            _free.Push(item);
        }
    }
}
