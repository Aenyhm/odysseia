using System;

namespace Sources.Toolbox {
    
    [Serializable]
    public struct SwapbackArray<T> {
        public T[] Items;
        public int Capacity;
        public int Count;

        public SwapbackArray(int initialCapacity) {
            Items = new T[initialCapacity];
            Count = 0;
            Capacity = initialCapacity;
        }

        public readonly bool CanAdd(int n = 1) => Count + n < Capacity;
        public void Add(T item) => Items[Count++] = item;
        public void Reset() => Count = 0;
        
        public void AddRange(T[] item) {
            foreach (var t in item) Add(t);
        }

        public void Resize(int newCapacity) {
            Capacity = newCapacity;
            var newArray = new T[Capacity];
            Array.Copy(Items, newArray, Count);
            Items = newArray;
        }
        
        public void RemoveAt(int index) {
            Items[index] = Items[Count - 1];
            Count--;
        }
  
        public void RemoveAll(Predicate<T> match) {
            for (var i = Count - 1; i >= 0; i--) {
                if (match(Items[i])) RemoveAt(i);
            }
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
        public int CoordsToIndex(Vec2I32 v) => CoordsToIndex(v.X, v.Y);
    }
}
