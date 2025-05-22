using UnityEngine;

namespace Unity.Scripts {
    public class TrunkEntityBehaviour : MonoBehaviour, IEntityBehaviour {
        public void Draw(in EntityView entityView, float dt) {
            transform.localPosition = entityView.Position;
        }
    }
}
