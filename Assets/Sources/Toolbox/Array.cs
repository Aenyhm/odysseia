using System;
using System.Collections;

namespace Sources.Toolbox {
    public struct SimpleArray<T> : IEnumerable {
        public readonly T[] Items;
        public int Count { get; private set; }
        private readonly int _capacity;
        private readonly bool _ordered;

        public SimpleArray(int capacity, bool ordered) {
            Items = new T[capacity];
            Count = 0;
            _capacity = capacity;
            _ordered = ordered;
        }

        public IEnumerator GetEnumerator() {
            for (var i = 0; i < Count; i++) {
                yield return Items[i];
            }
        }

        public bool CanAdd() => Count >= _capacity;
        public void Reset() => Count = 0;
        public void Add(T item) => Items[Count++] = item;

        public void RemoveAll(Predicate<T> match) {
            for (var i = Count - 1; i >= 0; i--) {
                var item = Items[i];
                if (match(item)) RemoveAt(i);
            }
        }
        
        public void RemoveAt(int index) {
            if (_ordered) OrderedRemoveAt(index);
            else SwapbackRemoveAt(index);
        }

        private void SwapbackRemoveAt(int index) {
            Items[index] = Items[Count - 1];
            Count--;
        }
        
        private void OrderedRemoveAt(int index) {
            for (var i = index; i < Count - 1; i++) {
                Items[i] = Items[i + 1];
            }
            Count--;
        }
    }
}
