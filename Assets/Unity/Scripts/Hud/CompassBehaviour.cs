using Sources.View;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class CompassBehaviour : MonoBehaviour, IViewRenderer {
        public void Render(in ViewState viewState) {
            transform.RotateOnAxis(Axis.Z, -viewState.WindView.Angle);
        }
    }
}
