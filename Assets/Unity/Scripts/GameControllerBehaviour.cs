using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public class GameControllerBehaviour : MonoBehaviour {
        [SerializeField] private ConfigurationScriptable _config;

        [Header("Debug")]
        [SerializeField][Range(0, 15)] private float _frameRate = 1f;
        [SerializeField] private GameState _gameState;

        private GameController _gameController;
        
        public SceneBehaviour CurrentScene { get; set; }

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start() {
            var conf = new RendererConf();
            conf.Sizes = new Dictionary<EntityType, Vec3F32>();
            foreach (var item in _config.CollidingObjects) {
                conf.Sizes.Add(item.entityType, GetVisualSize(item.gameObject));
            }
            
            // Doit être fait après le Awake pour que le SceneController passe avant.
            _gameController = new GameController(CurrentScene.SceneType, conf);
        }

        private void FixedUpdate() {
            _gameController.CoreUpdate(CurrentScene.SceneType, ReadInput(), Time.fixedDeltaTime*_frameRate);
            
            _gameState = _gameController.GameState;
        }
        
        private void Update() {
            CurrentScene.Render(in _gameState, Time.deltaTime*_frameRate);
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
        private static Vec3F32 GetVisualSize(GameObject go) {
            var meshRenderers = go.GetComponentsInChildren<MeshRenderer>();

            var bounds = meshRenderers[0].bounds;
            foreach (var mr in meshRenderers) {
                bounds.Encapsulate(mr.bounds);
            }

            return new Vec3F32(bounds.size.x, bounds.size.y, bounds.size.z);
        }
    }
}
