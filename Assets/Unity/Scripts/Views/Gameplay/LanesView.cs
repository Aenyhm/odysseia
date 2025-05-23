using Sources;

namespace Unity.Scripts.Views.Gameplay {
    public class LanesView : AbstractView {
        private float _defaultZ;
        
        private void Awake() {
            _defaultZ = transform.position.z;
        }
        
        public override void Render(in GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;

            transform.MoveOnAxis(Axis.Z, boat.Position.Z + _defaultZ);
        }
    }
}
