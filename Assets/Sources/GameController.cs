using Sources.Core;
using Sources.Scenes;
using Sources.Toolbox;

namespace Sources {
    public class GameController {
        private GameState _gameState;
        
        public GameState GameState => _gameState;

        public GameController(IPlatform platform, SceneType sceneType, in GameConf gameConf, in RendererConf rendererConf) {
            Services.Register(platform);
            Services.Register(gameConf);
            Services.Register(rendererConf);
            
            _gameState.GlobalProgression = FileStorage.Load<GlobalProgression>(CoreConfig.GlobalFileName);
            _gameState.PlayProgressions = FileStorage.LoadList<PlayProgression>(CoreConfig.PlayFileName);
            
            SceneManager.Register(SceneType.Gameplay, new GameplayScene());
            
            foreach (var scene in SceneManager.All) {
                scene.Init(ref _gameState);
            }
            
            SceneManager.GoTo(sceneType, ref _gameState);
        }
        
        public void CoreUpdate(SceneType sceneType, in GameInput input, float dt) {
            if (sceneType != _gameState.CurrentSceneType) {
                SceneManager.GoTo(sceneType, ref _gameState);
                _gameState.CurrentSceneType = sceneType;
            }
 
            SceneManager.Update(ref _gameState, in input, dt);
        }
    }
}
