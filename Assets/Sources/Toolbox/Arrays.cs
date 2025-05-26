using System;

namespace Sources.Toolbox {
    
    [Serializable]
    public struct SimpleArray<T> {
        public T[] Items;
        public int Capacity;
        public int Count;

        public SimpleArray(int initialCapacity) {
            Items = new T[initialCapacity];
            Count = 0;
            Capacity = initialCapacity;
        }

        public void Reset() => Count = 0;
        public bool CanAdd(int n = 1) => Count + n < Capacity;
        public void Add(T item) => Items[Count++] = item;
        
        public void AddRange(T[] item) {
            foreach (var t in item) Add(t);
        }

        public void Resize(int newCapacity) {
            Capacity = newCapacity;
            var newArray = new T[Capacity];
            Array.Copy(Items, newArray, Count);
            Items = newArray;
        }
        
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
    
    [Serializable]
    public struct SimpleGrid<T> {
        public T[] Items;
        public int Width;
        public int Height;
        
        public SimpleGrid(int width, int height) {
            Items = new T[width*height];
            Width = width;
            Height = height;
        }
        
        public int CoordsToIndex(int x, int y) => y*Width + x;
    }
}
