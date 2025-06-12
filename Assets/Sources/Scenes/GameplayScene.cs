using Sources.Core;

namespace Sources.Scenes {
    // Note: Un peu dommage qu'il n'y ait qu'une scène utilisée.
    
    // Mini-ECS (juste la partie systèmes)
    public class GameplayScene : IScene {
        private readonly GameState _gameState;
        
        public GameplayScene(GameState gameState) {
            _gameState = gameState;
        }
        
        public void Init() {
        }

        public void Update() {
            if (_gameState.PlayState.Mode != PlayMode.GameOver) {
                PauseSystem.Execute(_gameState);
            }
            
            if (_gameState.PlayState.Mode is PlayMode.Play or PlayMode.GameOver) {
                CannonballSystem.Execute(_gameState);
            }
            
            if (_gameState.PlayState.Mode == PlayMode.Play) {
                ScoreSystem.Execute(_gameState);
                AmmoSystem.Execute(_gameState);
                MermaidSystem.Execute(_gameState);
                JellyfishSystem.Execute(_gameState);
                ChangeLaneSystem.Execute(_gameState);
                BoatSystem.Execute(_gameState);
                CannonSystem.Execute(_gameState);
                RegionSystem.Execute(_gameState);
                RelicSystem.Execute(_gameState);
                WindSystem.Execute(_gameState);
                CoinSystem.Execute(_gameState);
                GameOverSystem.Execute(_gameState);
            }
            
            DestroySystem.Execute(_gameState);
        }
        
        public void Enter() {
            _gameState.PlayState = default;
            
            AmmoSystem.Init(_gameState);
            BoatSystem.Init(_gameState);
            CannonSystem.Init(_gameState);
            RegionSystem.Init(_gameState);
            WindSystem.Init(_gameState);
            
            _gameState.PlayState.PlayProgression = new PlayProgression();
            _gameState.PlayState.Mode = PlayMode.Play;
        }
        
        public void Exit() {}
    }
}
