using Sources.Core;
using UnityEngine;
using Transform = UnityEngine.Transform;

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
    
    public class BoatView : EntityView<Boat> {
        private Transform _sailPivotTransform;
        
        private void Start() {
            _sailPivotTransform = GameObject.Find("Mast").transform;
        }
        
        private void Update() {
            var targetRotation = Quaternion.Euler(_sailPivotTransform.rotation.x, -_entity.sailAngle, _sailPivotTransform.rotation.z);
            _sailPivotTransform.rotation = targetRotation;
        }
    }
}
