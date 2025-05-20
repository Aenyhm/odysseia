using Sources.States;
using Sources.Systems;

namespace Sources {
    public class Game {
        private GameState _gameState;
        
        public GameState GameState => _gameState;

        public Game(RendererConf rendererConf) {
            _gameState = new GameState();

            BoatSystem.Init(in rendererConf, out _gameState.Boat);
            RegionSystem.Init(in rendererConf, ref _gameState);
            WindSystem.Init(out _gameState.Wind);
        }
        
        public void CoreUpdate(float dt, in GameInput input) {
            BoatSystem.Update(ref _gameState.Boat, in input, in _gameState.Wind, _gameState.Region.Obstacles, dt);
            RegionSystem.Update(ref _gameState);
            WindSystem.Update(ref _gameState.Wind, in _gameState.Boat, dt);
        }
    }
}
