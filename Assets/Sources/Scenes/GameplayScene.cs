using Sources.Core;
using Sources.Toolbox;

namespace Sources.Scenes {
    public class GameplayScene : AbstractScene {
        public override void Init(ref GameState gameState) {
        }

        public override void Update(ref GameState gameState, in GameInput input) {
            var dt = Clock.DeltaTime;
            
            if (gameState.PlayState.Mode != PlayMode.GameOver) {
                PauseSystem.Execute(ref gameState.PlayState, in input);
            }
            
            if (gameState.PlayState.Mode is PlayMode.Play or PlayMode.GameOver) {
                CannonballSystem.Execute(ref gameState, dt);
            }
            
            if (gameState.PlayState.Mode == PlayMode.Play) {
                MermaidSystem.Execute(ref gameState);
                ChangeLaneSystem.Execute(ref gameState, in input, dt);
                BoatSystem.Execute(ref gameState, in input, dt);
                CannonballSystem.HandleCooldown(ref gameState, input, dt);
                RegionSystem.Execute(ref gameState);
                WindSystem.Execute(ref gameState, dt);
                CoinSystem.Execute(ref gameState);
                GameOverSystem.Execute(ref gameState);
            }
            
            MermaidSystem.Destroy(ref gameState);
            gameState.PlayState.Region.Entities.RemoveAll(e => e.Destroy);
        }
        
        public override void Enter(ref GameState gameState) {
            gameState.PlayState.Boat = BoatSystem.CreateBoat();
            CannonballSystem.Init(ref gameState);
            RegionSystem.Enter(ref gameState, RegionType.Aegis);
            WindSystem.Init(ref gameState);
            
            gameState.PlayState.PlayProgression = new PlayProgression();
            gameState.PlayState.Mode = PlayMode.Play;
        }
        
        public override void Exit(ref GameState gameState) {
        }
    }
}
