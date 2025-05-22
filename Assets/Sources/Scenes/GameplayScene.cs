using System.Collections.Generic;
using Sources.States;
using Sources.Systems;

namespace Sources.Scenes {
    public class GameplayScene : AbstractScene {
        private readonly List<AbstractSystem> _systems = new();
        
        public GameplayScene(in RendererConf rendererConf) {
            _systems.Add(new BoatSystem(rendererConf));
            _systems.Add(new RegionSystem(rendererConf));
            _systems.Add(new WindSystem());
            _systems.Add(new PauseSystem());
        }
        
        public override void Init(ref GameState gameState) {
            foreach (var system in _systems) {
                system.Init(ref gameState);
            }
        }

        public override void Update(ref GameState gameState, in GameInput input, float dt) {
            foreach (var system in _systems) {
                system.Update(ref gameState, in input, dt);
            }
        }
        
        public override void Enter(ref GameState gameState) {
            Init(ref gameState);
            gameState.GameMode = GameMode.Run;
        }
        
        public override void Exit(ref GameState gameState) {
        }
    }
}
