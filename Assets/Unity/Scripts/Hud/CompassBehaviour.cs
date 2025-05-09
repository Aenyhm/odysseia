using Sources;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class CompassBehaviour : MonoBehaviour {
        private const float ANIM_SPEED = 4f;
        
        private void Update() {
            var gs = Services.Get<GameState>();

            var targetRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -gs.wind.angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*ANIM_SPEED);
        }
    }
}
