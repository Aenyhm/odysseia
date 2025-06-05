using Sources;

namespace Unity.Scripts.Views.Gameplay {
    public class WindParticleView : AbstractView {
        private float _previousAngle;
        
        public override void Render(GameState gameState, float dt) {
            var wind = gameState.PlayState.Wind;
            var boat = gameState.PlayState.Boat;
            
            transform.MoveOnAxis(Axis.Z, boat.Position.Z - 5);
            transform.RotateOnAxis(Axis.Y, wind.CurrentAngle/5f);
        }
    }
}
