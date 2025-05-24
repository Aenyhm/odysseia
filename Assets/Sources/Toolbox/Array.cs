using System;

namespace Sources.Toolbox {
    
    [Serializable]
    public struct SimpleArray<T> {
        public T[] Items;
        public int Capacity;
        public int Count;

        public SimpleArray(int capacity) {
            Items = new T[capacity];
            Count = 0;
            Capacity = capacity;
        }

        public bool CanAdd() => Count >= Capacity;
        public void Reset() => Count = 0;
        public void Add(T item) => Items[Count++] = item;

        public void RemoveAtSwapback(int index) {
            Items[index] = Items[Count - 1];
            Count--;
        }
        
        public void RemoveAtOrdered(int index) {
            for (var i = index; i < Count - 1; i++) {
                Items[i] = Items[i + 1];
            }
            Count--;
        }
    }
}
