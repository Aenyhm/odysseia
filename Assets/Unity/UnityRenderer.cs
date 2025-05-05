using System;
using System.Collections.Generic;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity {
    public class UnityRenderer : IRenderer {
        private static readonly Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        
        private readonly Dictionary<Type, Type> _entityTypes = new() {
            { typeof(Camera3D), typeof(CameraView) },
            { typeof(Boat), typeof(BoatView) },
            { typeof(Obstacle), typeof(ObstacleView) },
        };

        private readonly Dictionary<Entity, GameObject> _gosByEntity = new();
        private readonly Pool<GameObject> _obstacleGoPool = new(() =>
            Object.Instantiate(Resources.Load<GameObject>("Cube"))
        );

        public void Create(Entity entity) {
            var entityType = entity.GetType();
            
            if (!_entityTypes.TryGetValue(entityType, out var viewType)) {
                throw new Exception($"No view type registered for {entityType.Name}");
            }

            var go = GetGameObjectForEntity(entity);
            go.name = $"Entity_{entity.id} ({entity.GetType().Name})";
            
            var view = (IView)go.AddComponent(viewType);
            view.SetEntity(entity);
            
            _gosByEntity.Add(entity, go);
        }
        
        public void Destroy(Entity entity) {
            var go = _gosByEntity[entity];
            go.SetActive(false);
            _gosByEntity.Remove(entity);
            
            if (entity is Obstacle) {
                _obstacleGoPool.Free(go);
            }
        }
        
        public void Update() {
            foreach (var (e, go) in _gosByEntity) {
                go.transform.localPosition = e.transform.position.ToUnityVector3();
                go.transform.localRotation = Quaternion.Euler(e.transform.rotation.ToUnityVector3());
                go.transform.localScale = e.transform.size.ToUnityVector3();
            }
        }
        
        private GameObject GetGameObjectForEntity(Entity entity) {
            return entity switch {
                Boat => Object.Instantiate(Resources.Load<GameObject>("Boat")),
                Camera3D => Camera.main!.gameObject,
                Obstacle => CreateObstacle(entity),
                _ => throw new Exception($"No GameObject for entity type: {entity.GetType().Name}")
            };
        }
        
        private GameObject CreateObstacle(Entity e) {
            var go = _obstacleGoPool.Get();
            var renderer = go.GetComponent<MeshRenderer>();
            if (!renderer) {
                go = Object.Instantiate(Resources.Load<GameObject>("Cube"));
                renderer.material = new Material(urpLitShader);
            }
            go.SetActive(true);

            renderer.material.color = new Color(e.color.x, e.color.y, e.color.z);
            
            return go;
        }
    }
}
