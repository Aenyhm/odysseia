using System;
using System.Collections.Generic;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Unity.Scripts {
    public struct Environment {
        public Camera camera;
        public Light light;
        public Renderer waterRenderer;
    }
    
    public class UnityRenderer {
        private readonly Dictionary<Entity, GameObject> _gosByEntity = new();
        private readonly Dictionary<Type, Pool<GameObject>> _poolsByType = new() {
            { typeof(Rock), new Pool<GameObject>(() => CreateGameObjectWithView("Rock")) },
            { typeof(Trunk), new Pool<GameObject>(() => CreateGameObjectWithView("Trunk")) }
        };
        private readonly Environment _environment;
        
        public static float deltaTime;

        public UnityRenderer(Environment environment) {
            _environment = environment;
            
            _environment.light.intensity = 1.0f;
            RenderSettings.fogDensity = UiConfig.FOG_DENSITY;
        }

        public void Link(Entity entity) {
            var go = GetEntityGameObject(entity);
            var view = go.GetComponent<IView>();
            view.SetEntity(entity);
            entity.transform.size = GetVisualSize(go);
            
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
        
        public void Update(float dt) {
            deltaTime = dt;
            
            foreach (var (e, go) in _gosByEntity) {
                go.transform.localPosition = e.transform.position.ToUnityVector3();
                go.transform.localRotation = Quaternion.Euler(e.transform.rotation.ToUnityVector3());
            }
        }

        public void ApplyTheme(UiConfig.RegionTheme theme) {
            _environment.camera.backgroundColor = theme.SkyColor.ToUnityColor();
            RenderSettings.fogColor = theme.SkyColor.ToUnityColor();
            _environment.light.color = theme.LightColor.ToUnityColor();
            _environment.light.intensity = theme.LightIntensity;

            var mat = _environment.waterRenderer.material;
            mat.color = theme.WaterColor.ToUnityColor();
        }

        private GameObject GetEntityGameObject(Entity e) {
            GameObject go;
            
            switch (e) {
                case Camera3D:
                    go = _environment.camera.gameObject;
                    AddViewToGameObject(go);
                    break;
                case Boat:
                    go = CreateGameObjectWithView("Boat", typeof(BoatView));
                    break;
                case Rock:
                    go = GetFromPool(e.GetType());
                    // TODO: on dedicated script
                    var prop = go.transform.GetChild(0);
                    prop.RotateOnAxis(Axis.Z, Services.Get<Random>().Next(0, 360));
                    break;
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
        
        private static Vec3F32 GetVisualSize(GameObject go) {
            var meshRenderers = go.GetComponentsInChildren<MeshRenderer>();

            if (meshRenderers.Length == 0)
                return Vec3F32.zero;

            var bounds = meshRenderers[0].bounds;
            foreach (var mr in meshRenderers) {
                bounds.Encapsulate(mr.bounds);
            }

            return new Vec3F32(bounds.size.x, bounds.size.y, bounds.size.z);
        }
    }
}
