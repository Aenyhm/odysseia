using System;
using System.Collections;
using System.Collections.Generic;

namespace Sources.Toolbox {

    [Serializable]
    public class SwapbackArray<T> : IEnumerable where T : struct {
        private const int _defaultCapacity = 256;

        public T[] Items;
        public int Capacity;
        public int Count;

        public SwapbackArray(int capacity = _defaultCapacity) {
            Items = new T[capacity];
            Capacity = capacity;
            Count = 0;
        }

        public void Reset() => Count = 0;

        public void Append(T item) {
            if (Count >= Capacity) {
                Resize(Capacity*2);
            }
            Items[Count++] = item;
        }

        public void Append(T[] item) {
            foreach (var t in item) Append(t);
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
        
        public T First(Predicate<T> match) {
            var result = default(T);
            
            for (var i = 0; i < Count; i++) {
                var item = Items[i];
                
                if (match(item)) {
                    result = item;
                    break;
                }
            }
            
            return result;
        }

        public IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < Count; i++) {
                yield return Items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void Resize(int newCapacity) {
            Services.Get<IPlatform>().LogWarn($"[SwapbackArray] Resizing {typeof(T).Name} from {Capacity} to {newCapacity}");
            Array.Resize(ref Items, newCapacity);
            Capacity = newCapacity;
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
        
        public void Reset() {
            for (var i = 0; i < Items.Length; i++) {
                Items[i] = default;
            }
        }
    }
}
