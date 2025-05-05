using Sources.Core;
using UnityEngine;

namespace Unity {
    public interface IView {
        void SetEntity(object entity);
    }
    
    public class EntityView<E> : MonoBehaviour, IView where E : Entity {
        [SerializeField] private E _entity;

        public void SetEntity(object entity) {
            _entity = (E)entity;
        }
    }
    
    public class BoatView : EntityView<Boat> { }
    public class CameraView : EntityView<Camera3D> { }
    public class ObstacleView : EntityView<Obstacle> { }
}
