using System.Collections.Generic;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views {
    
    // Gestion de plusieurs entités ayant le même prefab dans un Object Pool pour réutiliser les GameObjects,
    // car ils sont coûteux à instancier et détruire.
    public abstract class AbstractManagerView<T> : AbstractView {
        [SerializeField] protected GameObject _prefab;
        
        private Pool<GameObject> _pool;
        protected readonly Dictionary<int, GameObject> _gosById = new();
        private readonly string _label;
        private string _initialName;

        protected AbstractManagerView(string label) {
            _label = label;
        }

        private void Awake() {
            _pool = new Pool<GameObject>(() => Instantiate(_prefab, transform));
            _initialName = name;
        }
        
        protected abstract void InitChild(GameObject go, T data);

        protected void Sync(Dictionary<int, T> dataById) {
            // Remove
            var gosToRemove = new Dictionary<int, GameObject>(dataById.Count);
            foreach (var (goId, go) in _gosById) {
                var found = false;
                foreach (var dataId in dataById.Keys) {
                    if (dataId == goId) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    gosToRemove.Add(goId, go);
                }
            }
            foreach (var (id, go) in gosToRemove) {
                go.SetActive(false);
                _pool.Free(go);
                _gosById.Remove(id);
            }
            
            foreach (var (id, data) in dataById) {
                if (!_gosById.TryGetValue(id, out var go)) {
                    // Create
                    go = _pool.Get();
                    go.name = $"{_label}_{id}";
                    go.SetActive(true);
                    InitChild(go, data);
                    
                    _gosById[id] = go;
                }
            }
            
            name = $"{_initialName} ({_gosById.Count})";
        }
    }
}
