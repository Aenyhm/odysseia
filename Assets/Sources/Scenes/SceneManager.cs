using System.Collections.Generic;

namespace Sources.Scenes {
    public static class SceneManager {
        private static readonly Dictionary<SceneType, AbstractScene> _scenesByType = new();
        
        public static List<AbstractScene> All => new(_scenesByType.Values);
        
        public static void Register(SceneType sceneType, AbstractScene scene) {
            _scenesByType[sceneType] = scene;
        }
        
        public static AbstractScene Get(SceneType sceneType) {
            return _scenesByType[sceneType];
        }
        
        public static void GoTo(SceneType sceneType, ref GameState gameState) {
            _scenesByType[gameState.CurrentSceneType].Exit(ref gameState);
            gameState.CurrentSceneType = sceneType;
            _scenesByType[gameState.CurrentSceneType].Enter(ref gameState);
        }
    }
}
