using UnityEngine;

namespace Unity.Scripts {
    public class RockBehaviour : EntityBehaviour {
        [SerializeField] private Transform prop;
        
        private void OnEnable() {
            prop.RotateOnAxis(Axis.Z, Random.Range(0, 360));
        }
    }
}
