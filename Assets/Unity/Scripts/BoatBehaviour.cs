using Sources.View;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Unity.Scripts {
    public class BoatBehaviour : MonoBehaviour, IViewRenderer {
        [SerializeField] private Transform _sailTransform;

        public void Render(in ViewState viewState) {
            transform.localPosition = viewState.BoatView.Position.ToUnityVector3();
            _sailTransform.RotateOnAxis(Axis.Y, viewState.BoatView.SailAngle);
            _sailTransform.ScaleOnAxis(Axis.Z, viewState.BoatView.SailBow);
        }
    }
}
