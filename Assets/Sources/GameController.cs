using Sources.Core;
using Sources.Scenes;
using Sources.Toolbox;

namespace Sources {
    // Point d'entrée de la logique métier.
    public class GameController {
        public readonly GameState GameState = new();

        public GameController(IPlatform platform, SceneType sceneType, in GameConf gameConf, in RendererConf rendererConf) {
            Services.Register(platform);
            Services.Register(gameConf);
            Services.Register(rendererConf);
            
            GameState.GlobalProgression = FileStorage.Load<GlobalProgression>(CoreConfig.GlobalFileName);
            GameState.PlayProgressions = FileStorage.LoadList<PlayProgression>(CoreConfig.PlayFileName);
            
            SceneManager.Register(SceneType.Gameplay, new GameplayScene(GameState));
            
            foreach (var scene in SceneManager.All) {
                scene.Init();
            }
            
            SceneManager.GoTo(sceneType);
        }

        public void CoreUpdate(SceneType sceneType, in PlayerActions playerActions, float dt) {
            Clock.Update(dt);
            GameState.PlayerActions = playerActions;
            
            if (sceneType != GameState.CurrentSceneType) {
                SceneManager.GoTo(sceneType);
                GameState.CurrentSceneType = sceneType;
            }
 
            SceneManager.Update();
        }
    }
}
