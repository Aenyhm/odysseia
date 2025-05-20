using Sources.States;
using Sources.Systems;

namespace Sources {
    public class GameController {
        private GameState _gameState;
        
        public GameState GameState => _gameState;

        public GameController(RendererConf rendererConf) {
            _gameState = new GameState();

            BoatSystem.Init(in rendererConf, out _gameState.Boat);
            RegionSystem.Init(in rendererConf, ref _gameState);
            WindSystem.Init(out _gameState.Wind);
        }
        
        public void CoreUpdate(float dt, in GameInput input) {
            PauseSystem.Update(ref _gameState, in input);

            if (!_gameState.Pause) {
                BoatSystem.Update(ref _gameState, in input, in _gameState.Wind, in _gameState.Region, dt);
                RegionSystem.Update(ref _gameState);
                WindSystem.Update(ref _gameState.Wind, in _gameState.Boat, dt);
            }
        }
    }
}
