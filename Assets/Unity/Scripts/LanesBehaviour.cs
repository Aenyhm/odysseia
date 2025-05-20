using Sources.States;
using UnityEngine;

namespace Unity.Scripts {
    public class LanesBehaviour : MonoBehaviour, IViewRenderer {
        private float _defaultZ;
        
        private void Awake() {
            _defaultZ = transform.position.z;
        }
        
        public void Render(in GameState gameState, float dt) {
            transform.MoveOnAxis(Axis.Z, gameState.Boat.Position.Z + _defaultZ);
        }
    }
}
