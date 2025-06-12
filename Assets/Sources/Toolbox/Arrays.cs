using System;
using System.Collections;
using System.Collections.Generic;

namespace Sources.Toolbox {
    
    // Collection simple qui contient un tableau, sa capacité et le nombre d'éléments actuels.
    // Elle est performante car elle n'est pas ordonnée, et donc quand on supprime un élément, on met juste le dernier
    // élément à sa place et on décrémente le nombre d'éléments.
    // On peut préciser une capacité si on connait la taille max que le tableau va contenir.
    // Cette classe se comporte comme un tableau dynamique : si on dépasse la capacité, on fait une copie du tableau
    // dans un autre contenant 2x plus de place.
    // Elle n'accepte que des structs pour éviter d'aller chercher les éléments on ne sait où si ce sont des classes
    // (et donc des pointeurs vers un autre endroit en mémoire).
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
        
        // Note: Je trouvais pratique d'avoir cette méthode mais je ne l'utilise qu'une fois
        // et je ne l'aime pas trop car elle retourne une copie et pas l'élément original.
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

        // Permet de pouvoir utiliser foreach dessus avec inférence du type.
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator() {
            for (var i = 0; i < Count; i++) {
                yield return Items[i];
            }
        }

        private void Resize(int newCapacity) {
            Services.Get<IPlatform>().LogWarn($"[SwapbackArray] Resizing {typeof(T).Name} from {Capacity} to {newCapacity}");
            Array.Resize(ref Items, newCapacity);
            Capacity = newCapacity;
        }
    }

    // Représentation d'un tableau 2D (grille) avec un flat-array pour la même raison que précédemment : éviter d'avoir
    // du cache-miss car un tableau 2D est un tableau de tableaux et un tableau est juste une adresse mémoire.
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
