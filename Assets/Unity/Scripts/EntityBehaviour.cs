using Sources;
using UnityEngine;

namespace Unity.Scripts {
    public struct EntityView {
        public Vector3 Position;
        public int Id;
        public EntityType Type;
    }
    
    public interface IEntityBehaviour {
        void Draw(in EntityView entityView);
    }
    
    public class EntityBehaviour : MonoBehaviour, IEntityBehaviour {
        public void Draw(in EntityView entityView) {
            transform.localPosition = entityView.Position;
        }
    }
}
