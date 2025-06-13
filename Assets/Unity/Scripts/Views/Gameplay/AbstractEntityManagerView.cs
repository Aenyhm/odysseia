using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public abstract class AbstractEntityManagerView : AbstractManagerView<Entity> {
        protected AbstractEntityManagerView(string label) : base(label) { }
        
        protected override int GetId(Entity data) => data.Id;

        protected override void InitChild(GameObject go, Entity data) {
        }
        
        protected override void UpdateChild(GameObject go, Entity data) {
            go.transform.localPosition = data.Position.ToUnityVector3();
        }
    }
}
