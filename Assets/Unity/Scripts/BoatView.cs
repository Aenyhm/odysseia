using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;
using Transform = UnityEngine.Transform;

namespace Unity.Scripts {
    public class BoatView : EntityView<Boat> {
        private const float SAIL_DEFAULT_Z = 0.4f;
        private const float SAIL_WINDWARD_Z = 1.6f;
        
        private Transform _sailTransform;
        
        private void Start() {
            _sailTransform = GameObject.Find("Sail").transform;
        }
        
        private void Update() {
            var gs = Services.Get<GameState>();

            _sailTransform.RotateOnAxis(Axis.Y, _entity.sailAngle);
            
            var targetScaleZ = BoatController.IsSailWindward(gs.boat, gs.wind) ? SAIL_WINDWARD_Z : SAIL_DEFAULT_Z;
            var newScaleZ = Maths.MoveTowards(_sailTransform.localScale.z, targetScaleZ, UnityRenderer.deltaTime);
            _sailTransform.ScaleOnAxis(Axis.Z, newScaleZ);
        }
    }
}
