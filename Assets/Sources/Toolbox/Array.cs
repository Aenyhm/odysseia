using System;

namespace Sources.Toolbox {
    
    [Serializable]
    public struct SwapbackArray<T> {
        public readonly T[] Items;
        public readonly int Capacity;
        public int Count { get; private set; }

        public SwapbackArray(int capacity) {
            Items = new T[capacity];
            Count = 0;
            Capacity = capacity;
        }

        public bool CanAdd() => Count >= Capacity;
        public void Reset() => Count = 0;
        public void Add(T item) => Items[Count++] = item;

        public void RemoveAt(int index) {
            Items[index] = Items[Count - 1];
            Count--;
        }
    }
}
