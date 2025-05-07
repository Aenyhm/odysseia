using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity {
    public class UnityRenderer {
        private readonly Dictionary<Entity, GameObject> _gosByEntity = new();
        private readonly Dictionary<Type, Pool<GameObject>> _poolsByType = new() {
            { typeof(Rock), new Pool<GameObject>(() => CreateGameObjectWithView("Rock")) },
            { typeof(Trunk), new Pool<GameObject>(() => CreateGameObjectWithView("Trunk")) }
        };

        public void Link(Entity entity) {
            var go = GetEntityGameObject(entity);
            var view = go.GetComponent<IView>();
            view.SetEntity(entity);
            
            _gosByEntity.Add(entity, go);
        }
        
        public void Unlink(Entity entity) {
            var go = _gosByEntity[entity];
            go.SetActive(false);
            _gosByEntity.Remove(entity);
            
            if (_poolsByType.TryGetValue(entity.GetType(), out var pool)) {
                pool.Free(go);
            }
        }
        
        public void Update() {
            foreach (var (e, go) in _gosByEntity) {
                go.transform.localPosition = e.transform.position.ToUnityVector3();
                go.transform.localRotation = Quaternion.Euler(e.transform.rotation.ToUnityVector3());
                go.transform.localScale = e.transform.size.ToUnityVector3();
            }
        }
        
        private GameObject GetEntityGameObject(Entity e) {
            GameObject go;
            
            switch (e) {
                case Camera3D:
                    go = Camera.main!.gameObject;
                    AddViewToGameObject(go);
                    break;
                case Boat:
                    go = CreateGameObjectWithView("Boat", typeof(BoatView));
                    break;
                case Rock:
                case Trunk:
                    go = GetFromPool(e.GetType());
                    break;
                default:
                    throw new Exception($"No GameObject for entity type: {e.GetType().Name}");
            }
            
            return go;
        }
        
        private static GameObject CreateGameObjectWithView(string assetName, Type viewType = null) {
            var go = Object.Instantiate(Resources.Load<GameObject>(assetName));
            AddViewToGameObject(go, viewType);
            
            return go;
        }
        
        private static void AddViewToGameObject(GameObject go, Type viewType = null) {
            go.AddComponent(viewType ?? typeof(GenericView));
        }
        
        private GameObject GetFromPool(Type entityType) {
            var go = _poolsByType[entityType].Get();
            go.SetActive(true);
            return go;
        }
    }
}
