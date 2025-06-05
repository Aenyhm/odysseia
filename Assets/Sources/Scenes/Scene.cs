using System.Collections.Generic;

namespace Sources.Scenes {
    public interface IScene {
        void Init();
        void Update();
        void Enter();
        void Exit();
    }
    
    public static class SceneManager {
        private static readonly Dictionary<SceneType, IScene> _scenesByType = new();
        private static IScene _currentScene;
        
        public static List<IScene> All => new(_scenesByType.Values);
        
        public static void Register(SceneType sceneType, IScene scene) {
            _scenesByType[sceneType] = scene;
        }

        public static void GoTo(SceneType sceneType) {
            _currentScene?.Exit();
            
            if (_scenesByType.TryGetValue(sceneType, out _currentScene)) {
                _currentScene.Enter();
            }
        }
        
        public static void Update() {
            _currentScene?.Update();
        }
    }
}
