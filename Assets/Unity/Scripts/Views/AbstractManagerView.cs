using System.Collections.Generic;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views {
    public abstract class AbstractManagerView<T> : AbstractView {
        [SerializeField] protected GameObject _prefab;
        
        private Pool<GameObject> _pool;
        protected readonly Dictionary<int, GameObject> _gosById = new();
        private readonly string _label;
        private readonly bool _createParent;

        protected AbstractManagerView(string label, bool createParent = true) {
            _label = label;
            _createParent = createParent;
        }

        private void Awake() {
            Transform parentTransform;
            if (_createParent) {
                var parent = new GameObject($"{_label}s");
                parent.transform.parent = transform;
                parentTransform = parent.transform;
            } else {
                parentTransform = transform;
            }
            
            _pool = new Pool<GameObject>(() => Instantiate(_prefab, parentTransform));
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
        }
    }
}
