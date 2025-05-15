using Sources.View;
using UnityEngine;

namespace Unity.Scripts {
    public interface IEntityBehaviour {
        void Draw(in EntityView entityView);
    }
    
    public class EntityBehaviour : MonoBehaviour, IEntityBehaviour {
        public void Draw(in EntityView entityView) {
            transform.localPosition = entityView.Position.ToUnityVector3();
        }
    }
}
