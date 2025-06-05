using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using Unity.Scripts.Scriptables;
using UnityEngine;

namespace Unity.Scripts {
    public class GameControllerBehaviour : MonoBehaviour {
        [SerializeField] private TweaksScriptable _tweaksScriptable;
        [SerializeField] private CollidingScriptable _collidingScriptable;

        [Header("Debug")]
        [SerializeField][Range(0, 15)] private float _frameRate = 1f;
        [SerializeField] public GameState GameState;

        private GameController _gameController;
        
        public SceneBehaviour CurrentScene { get; set; }
        public int TitleCoinCount { get; set; }

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        
        public void Init(SceneType sceneType) {
            var rendererConf = new RendererConf();

            rendererConf.Sizes = new Dictionary<EntityType, Vec3F32>();
            foreach (var item in _collidingScriptable.CollidingObjects) {
                rendererConf.Sizes.Add(item.entityType, GetVisualSize(item.gameObject));
            }
            
            rendererConf.SegmentsByRegion = new Dictionary<RegionType, List<Segment>>();
            foreach (RegionType regionType in Enum.GetValues(typeof(RegionType))) {
                rendererConf.SegmentsByRegion.Add(regionType, new List<Segment>());
                
                var segmentScriptables = Resources.LoadAll<SegmentScriptable>($"Segments/{regionType}/");
                foreach (var item in segmentScriptables) {
                    rendererConf.SegmentsByRegion[regionType].Add(item.Segment);
                }
            }

            _gameController = new GameController(
                new UnityPlatform(),
                sceneType,
                _tweaksScriptable.GameConf,
                rendererConf
            );

            TitleCoinCount = _gameController.GameState.GlobalProgression.CoinCount;
        }

        private void FixedUpdate() {
            _gameController.CoreUpdate(CurrentScene.SceneType, UnityInput.Data, Time.fixedDeltaTime*_frameRate);
            UnityInput.Clear();
            
            GameState = _gameController.GameState;
        }
        
        private void Update() {
            // L'input doit être calculé dans l'Update et non dans le FixedUpdate
            // pour qu'il soit check à chaque frame, pas moins.
            UnityInput.Update();

            CurrentScene.Render(GameState, Time.deltaTime*_frameRate);
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
