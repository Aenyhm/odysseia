using Sources.Scenes;
using Sources.States;

namespace Sources {
    public class GameController {
        private GameState _gameState;
        
        public GameState GameState => _gameState;

        public GameController(SceneType sceneType, in RendererConf rendererConf) {
            SceneManager.Register(SceneType.Title, new TitleScene());
            SceneManager.Register(SceneType.Gameplay, new GameplayScene(in rendererConf));
            
            foreach (var scene in SceneManager.All) {
                scene.Init(ref _gameState);
            }
            
            SceneManager.GoTo(sceneType, ref _gameState);
        }
        
        public void CoreUpdate(SceneType sceneType, in GameInput input, float dt) {
            if (sceneType != _gameState.CurrentSceneType) {
                SceneManager.GoTo(sceneType, ref _gameState);
            }
            
            SceneManager.Get(_gameState.CurrentSceneType).Update(ref _gameState, in input, dt);
        }
    }
}
