using System.Collections.Generic;

namespace Sources.Scenes {
    public static class SceneManager {
        private static readonly Dictionary<SceneType, AbstractScene> _scenesByType = new();
        private static AbstractScene _currentScene;
        
        public static List<AbstractScene> All => new(_scenesByType.Values);
        
        public static void Register(SceneType sceneType, AbstractScene scene) {
            _scenesByType[sceneType] = scene;
        }

        public static AbstractScene Get(SceneType sceneType) {
            return _scenesByType[sceneType];
        }
        
        public static void GoTo(SceneType sceneType, ref GameState gameState) {
            _currentScene?.Exit(ref gameState);
            
            if (_scenesByType.TryGetValue(sceneType, out _currentScene)) {
                _currentScene.Enter(ref gameState);
            }
        }
        
        public static void Update(ref GameState gameState, in GameInput input) {
            _currentScene?.Update(ref gameState, in input);
        }
    }
}
