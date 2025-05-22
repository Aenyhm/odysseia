using UnityEngine;

namespace Unity.Scripts {
    public class CoinEntityBehaviour : MonoBehaviour, IEntityBehaviour {
        [SerializeField] private float _rotateSpeed;
        
        public void Draw(in EntityView entityView, float dt) {
            transform.localPosition = entityView.Position;
            transform.RotateOnAxis(Axis.Y, transform.localEulerAngles.y + dt*_rotateSpeed);
        }
    }
}
