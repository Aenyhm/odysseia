using UnityEngine;

namespace Unity.Scripts {
    public class RockBehaviour : EntityBehaviour {
        [SerializeField] private Transform prop;
        
        public void Start() {
            prop.RotateOnAxis(Axis.Z, Random.Range(0, 360)); // TODO: random value in view once
        }
    }
}
