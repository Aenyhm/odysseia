using Sources.Core;

namespace Sources.Scenes {
    public class GameplayScene : AbstractScene {
        public override void Init(ref GameState gameState) {
        }

        public override void Update(ref GameState gameState, in GameInput input, float dt) {
            if (gameState.PlayState.Mode != PlayMode.GameOver) {
                PauseSystem.Execute(ref gameState.PlayState, in input);
            }
            
            if (gameState.PlayState.Mode == PlayMode.Play) {
                BoatSystem.Execute(ref gameState.PlayState, input, dt);
                RegionSystem.Execute(ref gameState.PlayState);
                WindSystem.Execute(ref gameState.PlayState, dt);
                CoinSystem.Execute(ref gameState.PlayState);
                GameOverSystem.Execute(ref gameState);
            }
        }
        
        public override void Enter(ref GameState gameState) {
            gameState.PlayState.Boat = BoatSystem.CreateBoat();
            gameState.PlayState.Wind = WindSystem.CreateWind();
            RegionSystem.Enter(ref gameState.PlayState, RegionType.Aegis);
            gameState.PlayState.Mode = PlayMode.Play;
        }
        
        public override void Exit(ref GameState gameState) {
        }
    }
}
