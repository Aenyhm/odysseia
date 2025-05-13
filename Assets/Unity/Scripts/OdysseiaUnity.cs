using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public class OdysseiaUnity : MonoBehaviour, IPlatform {
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _light;
        [SerializeField] private Renderer _waterRenderer;
        [SerializeField] private RegionType _regionType;
        [SerializeField] private float _frameRate = 1f;

        private UnityRenderer _renderer;
        private Game _game;
        
        public void Log(string message) {
            Debug.Log(message);
        }

        public void AddEntityView(Entity entity) {
            _renderer.Link(entity);
        }

        public void RemoveEntityView(Entity entity) {
            _renderer.Unlink(entity);
        }

        private void Start() {
            var environment = new Environment {
                camera = _camera,
                light = _light,
                waterRenderer = _waterRenderer,
            };
            _renderer = new UnityRenderer(environment);
            _game = new Game(this);
            
            _regionType = Services.Get<GameState>().region.type;
            var regionTheme = Services.Get<UiConfig>().regionThemes[_regionType];      
            _renderer.ApplyTheme(regionTheme);
        }

        private void FixedUpdate() {
            _game.Update(Time.fixedDeltaTime*_frameRate, ReadInput());
        }
        
        private void Update() {
            _renderer.Update(Time.deltaTime*_frameRate);
        }
        
        private static GameInput ReadInput() {
            GameInput input;
            input.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            input.MouseDeltaX = Input.mousePositionDelta.x;
            input.MouseButtonLeftDown = Input.GetMouseButton(0);
            return input;
        }
    }
}
