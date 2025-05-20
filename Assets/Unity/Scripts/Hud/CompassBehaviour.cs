using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class CompassBehaviour : MonoBehaviour, IViewRenderer {
        public void Render(in GameState gameState, float dt) {
            transform.RotateOnAxis(Axis.Z, -gameState.Wind.Angle);
        }
    }
}
