using Sources.View;
using UnityEngine;

namespace Unity.Scripts {
    public class LanesBehaviour : MonoBehaviour, IViewRenderer {
        private float _defaultZ;
        
        private void Awake() {
            _defaultZ = transform.position.z;
        }
        
        public void Render(in ViewState viewState) {
            transform.MoveOnAxis(Axis.Z, viewState.BoatView.Position.Z + _defaultZ);
        }
    }
}
