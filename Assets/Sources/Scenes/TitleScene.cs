using Sources.States;

namespace Sources.Scenes {
	public class TitleScene : AbstractScene {
        public override void Init(ref GameState gameState) {
        }

        public override void Update(ref GameState gameState, in GameInput input, float dt) {
        }
        
        public override void Enter(ref GameState gameState) {
            gameState.RunCoinCount = 0;
        }
        
        public override void Exit(ref GameState gameState) {
        }
    }
}
