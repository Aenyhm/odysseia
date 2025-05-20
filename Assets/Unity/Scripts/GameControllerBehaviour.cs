using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources;
using Sources.States;
using Sources.Toolbox;
using Unity.Scripts.Views;
using UnityEngine;

namespace Unity.Scripts {
    public class GameControllerBehaviour : MonoBehaviour {
        [SerializeField] private ConfigurationScriptable _config;
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _light;
        [SerializeField] private Renderer _waterRenderer;
        [SerializeField] private GameObject[] _viewsObjects;

        [Header("Debug")]
        [SerializeField][Range(0, 15)] private float _frameRate = 1f;
        [SerializeField] private GameState _gameState;

        private IEnumerable<IView> _views;
        private GameController _gameController;

        private void Awake() {
            _views = GetViewsByGameObject(_viewsObjects);
            
            var conf = new RendererConf();
            conf.Sizes = new Dictionary<EntityType, Vec3F32>();
            foreach (var item in _config.CollidingObjects) {
                conf.Sizes.Add(item.entityType, GetVisualSize(item.gameObject));
            }

            _gameController = new GameController(conf);
        }

        private void FixedUpdate() {
            _gameController.CoreUpdate(Time.fixedDeltaTime*_frameRate, ReadInput());
        }
        
        private void Update() {
            _gameState = _gameController.GameState;
            
            var regionTheme = ViewConfig.regionThemesByType[_gameState.Region.Type];
            ApplyRegionTheme(regionTheme);
            
            var dt = Time.deltaTime*_frameRate;

            foreach (var view in _views) {
                view.Render(in _gameState, dt);
            }
        }
        
        private static GameInput ReadInput() {
            GameInput input;
            input.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            input.MouseDeltaX = Input.mousePositionDelta.x;
            input.MouseButtonLeftDown = Input.GetMouseButton(0);
            input.Escape = Input.GetKey(KeyCode.Escape);
            return input;
        }
        
        [Pure]
        private static List<IView> GetViewsByGameObject(GameObject[] gameObjects) {
            var result = new List<IView>();

            foreach (var go in gameObjects) {
                var view = go.GetComponent<IView>();
                if (view != null) {
                    result.Add(view);
                } else {
                    Debug.LogWarning($"{go.name} n'a pas de composant qui implémente IView");
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
