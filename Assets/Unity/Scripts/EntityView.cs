using Sources.Core;
using UnityEngine;

namespace Unity.Scripts {
    public interface IView {
        void SetEntity(object entity);
    }
    
    public class EntityView<E> : MonoBehaviour, IView where E : Entity {
        [SerializeField] protected E _entity;

        public void SetEntity(object entity) {
            _entity = (E)entity;
            name = $"Entity_{_entity.id} ({_entity.GetType().Name})";
        }
    }
    
    public class GenericView : EntityView<Entity> { }
    public class BoatView : EntityView<Boat> { }
}
