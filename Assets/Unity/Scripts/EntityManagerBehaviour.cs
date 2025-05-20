using System.Collections.Generic;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public abstract class EntityManagerBehaviour : MonoBehaviour {
        [SerializeField] protected GameObject _prefab;
        
        private Pool<GameObject> _pool;
        private readonly Dictionary<int, GameObject> _gosById = new();

        private void Awake() {
             _pool = new Pool<GameObject>(() => Instantiate(_prefab, transform));
        }
        
        protected void Sync(List<EntityView> entityViews) {
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
                    go.name = $"{entityView.Type}_{entityView.Id}";
                    go.SetActive(true);
                    
                    _gosById[entityView.Id] = go;
                }
                
                // Update
                go.GetComponent<IEntityBehaviour>().Draw(entityView); // @perf
            }
        }
    }
}
