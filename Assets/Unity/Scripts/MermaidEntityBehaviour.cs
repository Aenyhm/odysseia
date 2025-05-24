using UnityEngine;

namespace Unity.Scripts {
    public class MermaidEntityBehaviour : MonoBehaviour, IEntityBehaviour {
        public void Draw(in EntityView entityView, float dt) {
            transform.localPosition = entityView.Position;
        }
    }
}
