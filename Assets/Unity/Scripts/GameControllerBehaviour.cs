using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using Unity.Scripts.Scriptables;
using UnityEngine;

namespace Unity.Scripts {
    // Point d'entrée du jeu, est créé automatiquement sur la scène qui se lance en premier via SceneBehaviour.
    public class GameControllerBehaviour : MonoBehaviour {
        [SerializeField] private TweaksScriptable _tweaksScriptable;
        [SerializeField] private CollidingScriptable _collidingScriptable;
        [SerializeField] private int _targetFps = 60;

        [Header("Debug")]
        [SerializeField][Range(0, 15)] private float _frameRate = 1f;
        [SerializeField] public GameState GameState;

        private GameController _gameController;

        public SceneBehaviour CurrentScene { get; set; }
        public int TitleCoinCount { get; set; }
        public bool ShowControls { get; private set; }

        public static GameControllerBehaviour Instance;

        private void Awake() {
            DontDestroyOnLoad(this);
            Instance = this;
            name = "GameController";
            
            Application.targetFrameRate = _targetFps;
        }
        
        public void Init(SceneType sceneType) {
            var rendererConf = new RendererConf();

            rendererConf.BoundingBoxesByEntityType = new Dictionary<EntityType, BoundingBox3F32>();
            foreach (var item in _collidingScriptable.CollidingObjects) {
                rendererConf.BoundingBoxesByEntityType.Add(item.entityType, GetBoundingBox(item.gameObject));
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

            LoadPrefs();
        }

        private void FixedUpdate() {
            if (UnityInput.Instance.Actions.ShowControlsSwitched) {
                SwitchShowControls();
            }
            
            _gameController.CoreUpdate(CurrentScene.SceneType, UnityInput.Instance.Actions, Time.fixedDeltaTime*_frameRate);
            UnityInput.Instance.Clear();
            
            GameState = _gameController.GameState;
        }
        
        private void Update() {
            // L'input doit être calculé dans l'Update et non dans le FixedUpdate
            // pour qu'il soit check à chaque frame, pas moins.
            UnityInput.Instance.Update();

            CurrentScene.Render(GameState, Time.deltaTime*_frameRate);
        }

        [Pure]
        private static BoundingBox3F32 GetBoundingBox(GameObject go) {
            var collider = go.GetComponent<BoxCollider>();
            if (!collider) {
                collider = go.GetComponentInChildren<BoxCollider>();
            }
            
            var centerWorld = collider.transform.TransformPoint(collider.center);
            var sizeWorld = Vector3.Scale(collider.size, collider.transform.lossyScale);

            var min = centerWorld - sizeWorld*0.5f;
            var max = centerWorld + sizeWorld*0.5f;

            return new BoundingBox3F32 {
                Min = min.FromUnityVector3(),
                Max = max.FromUnityVector3(),
            };
        }

        public void SwitchShowControls() {
            ShowControls = !ShowControls;
            PlayerPrefs.SetInt("ShowControls", ShowControls ? 1 : 0);
        }

        private void LoadPrefs() {
            if (PlayerPrefs.HasKey("AudioVolume")) {
                AudioListener.volume = PlayerPrefs.GetFloat("AudioVolume");
            } else {
                AudioListener.volume = 1.0f;
                PlayerPrefs.SetFloat("AudioVolume", AudioListener.volume);
            }
            
            if (PlayerPrefs.HasKey("ShowControls")) {
                ShowControls = PlayerPrefs.GetInt("ShowControls") == 1;
            }
        }
    }
}
