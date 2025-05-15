using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using Sources.View;
using UnityEngine;

namespace Unity.Scripts {
    public class GameBehaviour : MonoBehaviour, IPlatform {
        [SerializeField] private ConfigurationScriptable _configuration;
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _light;
        [SerializeField] private Renderer _waterRenderer;
        [SerializeField] private GameObject[] _rendererObjects;
    
        [Header("Debug")]
        [SerializeField] private float _frameRate = 1f;

        private IEnumerable<IViewRenderer> _viewRenderers;
        private Game _game;
        
        public void Log(string message) {
            Debug.Log(message);
        }

        private void Awake() {
            var viewRenderersByGameObject = GetViewRenderersByGameObject(_rendererObjects);
            _viewRenderers = viewRenderersByGameObject.Values;
            
            var conf = new Conf();
            conf.Sizes = new Dictionary<EntityType, Vec3F32>();
            
            foreach (var item in _configuration.CollidingObjects) {
                conf.Sizes.Add(item.entityType, GetVisualSize(item.gameObject));
            }
            
            _game = new Game(this, conf);
        }

        private void FixedUpdate() {
            _game.CoreUpdate(Time.fixedDeltaTime*_frameRate, ReadInput());
        }
        
        private void Update() {
            _game.ViewUpdate(Time.deltaTime*_frameRate, out var viewState);

            ApplyRegionTheme(viewState.RegionTheme);
            
            foreach (var viewRenderer in _viewRenderers) {
                viewRenderer.Render(in viewState);
            }
        }
        
        private static GameInput ReadInput() {
            GameInput input;
            input.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            input.MouseDeltaX = Input.mousePositionDelta.x;
            input.MouseButtonLeftDown = Input.GetMouseButton(0);
            return input;
        }
        
        [Pure]
        private static Dictionary<GameObject, IViewRenderer> GetViewRenderersByGameObject(GameObject[] gameObjects) {
            var result = new Dictionary<GameObject, IViewRenderer>();

            foreach (var go in gameObjects) {
                var renderer = go.GetComponent<IViewRenderer>();
                if (renderer != null) {
                    result.Add(go, renderer);
                } else {
                    Debug.LogWarning($"{go.name} n'a pas de composant qui implémente IViewRenderer");
                }
            }

            return result;
        }
        
        [Pure]
        private static Vec3F32 GetVisualSize(GameObject go) {
            var meshRenderers = go.GetComponentsInChildren<MeshRenderer>();

            var bounds = meshRenderers[0].bounds;
            foreach (var mr in meshRenderers) {
                bounds.Encapsulate(mr.bounds);
            }

            return new Vec3F32(bounds.size.x, bounds.size.y, bounds.size.z);
        }

        private void ApplyRegionTheme(RegionTheme theme) {
            RenderSettings.fogDensity = ViewConfig.FOG_DENSITY;
            RenderSettings.fogColor = theme.SkyColor.ToUnityColor();
            
            _camera.backgroundColor = theme.SkyColor.ToUnityColor();
            
            _light.color = theme.LightColor.ToUnityColor();
            _light.intensity = theme.LightIntensity;

            var waterMaterial = _waterRenderer.material;
            waterMaterial.color = theme.WaterColor.ToUnityColor();
        }
    }
}
