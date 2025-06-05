using Sources;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CompassView : AbstractView {
        public override void Render(GameState gameState, float dt) {
            var wind = gameState.PlayState.Wind;
            
            transform.RotateOnAxis(Axis.Z, -wind.CurrentAngle);
        }
    }
}
