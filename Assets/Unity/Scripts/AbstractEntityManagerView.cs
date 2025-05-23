using System.Collections.Generic;
using Sources;
using Sources.Toolbox;
using Unity.Scripts.Views;
using UnityEngine;

namespace Unity.Scripts {
    public abstract class AbstractEntityManagerView : AbstractView {
        [SerializeField] protected GameObject _prefab;
        
        private Pool<GameObject> _pool;
        private readonly Dictionary<int, GameObject> _gosById = new();

        private void Awake() {
             _pool = new Pool<GameObject>(() => Instantiate(_prefab, transform));
        }
        
        protected abstract EntityType Type { get; }
        
        protected void Sync(List<EntityView> entityViews, float dt) {
            // Remove
            var gosToRemove = new Dictionary<int, GameObject>();
            foreach (var (id, go) in _gosById) {
                var found = false;
                foreach (var entityView in entityViews) {
                    if (entityView.Id == id) {
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

            foreach (var entityView in entityViews) {
                if (!_gosById.TryGetValue(entityView.Id, out var go)) {
                    // Create
                    go = _pool.Get();
                    go.name = $"{Type}_{entityView.Id}";
                    go.SetActive(true);
                    
                    _gosById[entityView.Id] = go;
                }
                
                // Update
                go.GetComponent<IEntityBehaviour>().Draw(entityView, dt); // @perf
            }
        }
    }
}
