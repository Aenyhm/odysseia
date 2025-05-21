using Sources.States;

namespace Unity.Scripts.Views.Gameplay {
    public class LanesView : AbstractView {
        private float _defaultZ;
        
        private void Awake() {
            _defaultZ = transform.position.z;
        }
        
        public override void Render(in GameState gameState, float dt) {
            transform.MoveOnAxis(Axis.Z, gameState.Boat.Position.Z + _defaultZ);
        }
    }
}
