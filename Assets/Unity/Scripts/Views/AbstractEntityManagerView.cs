using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views {
    public abstract class AbstractEntityManagerView : AbstractView {
        [SerializeField] protected GameObject _prefab;
        
        private Pool<GameObject> _pool;
        protected readonly Dictionary<int, GameObject> _gosById = new();

        private void Awake() {
            var parent = new GameObject($"{Type}s");
            parent.transform.parent = transform;
            
             _pool = new Pool<GameObject>(() => Instantiate(_prefab, parent.transform));
        }
        
        protected abstract EntityType Type { get; }
        
        protected virtual void Init(GameObject go) {}

        protected void Sync(ICollection<int> entityIds) {
            // Remove
            var gosToRemove = new Dictionary<int, GameObject>(entityIds.Count);
            foreach (var (id, go) in _gosById) {
                var found = false;
                foreach (var entityId in entityIds) {
                    if (entityId == id) {
                        found = true;
                        break;
                    }
                }
                if (!found) {
                    gosToRemove.Add(id, go);
                }
            }
            foreach (var (id, go) in gosToRemove) {
                go.SetActive(false);
                _pool.Free(go);
                _gosById.Remove(id);
            }
            
            foreach (var entityId in entityIds) {
                if (!_gosById.TryGetValue(entityId, out var go)) {
                    // Create
                    go = _pool.Get();
                    go.name = $"{Type}_{entityId}";
                    go.SetActive(true);
                    Init(go);
                    
                    _gosById[entityId] = go;
                }
            }
        }
    }
}
