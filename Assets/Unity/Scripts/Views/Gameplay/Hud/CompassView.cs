using Sources.States;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CompassView : AbstractView {
        public override void Render(in GameState gameState, float dt) {
            transform.RotateOnAxis(Axis.Z, -gameState.Wind.Angle);
        }
    }
}
