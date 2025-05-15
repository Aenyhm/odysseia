using Sources.View;
using UnityEngine;

namespace Unity.Scripts {
    public class CameraBehaviour : MonoBehaviour, IViewRenderer {
        public void Render(in ViewState viewState) {
            transform.MoveOnAxis(Axis.Z, viewState.CameraView.Position.Z);
        }
    }
}
