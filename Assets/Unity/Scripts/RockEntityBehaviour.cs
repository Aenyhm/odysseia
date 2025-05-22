using UnityEngine;

namespace Unity.Scripts {
    public class RockEntityBehaviour : MonoBehaviour, IEntityBehaviour {
        [SerializeField] private Transform prop;
        
        private void OnEnable() {
            prop.RotateOnAxis(Axis.Z, Random.Range(0, 360));
        }

        public void Draw(in EntityView entityView, float dt) {
            transform.localPosition = entityView.Position;
        }
    }
}
