using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Views.Hud {
    public class CompassView : MonoBehaviour, IView {
        public void Render(in GameState gameState, float dt) {
            transform.RotateOnAxis(Axis.Z, -gameState.Wind.Angle);
        }
    }
}
