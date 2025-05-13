using Sources;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class CompassBehaviour : MonoBehaviour {
        private void Update() {
            var gs = Services.Get<GameState>();

            transform.RotateOnAxis(Axis.Z, -gs.wind.angle);
        }
    }
}
