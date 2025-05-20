using Sources.States;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views {
    public class BoatView : MonoBehaviour, IView {
        private const float _minSailBow = 0.35f;
        private const float _maxSailBow = 0.75f;
        
        [SerializeField] private Transform _sailTransform;
        
        private float _sailBow = _minSailBow;

        public void Render(in GameState gameState, float dt) {
            var boat = gameState.Boat;
            
            var targetSailBow = boat.SailWindward ? _maxSailBow : _minSailBow;
            _sailBow = Maths.MoveTowards(_sailBow, targetSailBow, dt);

            transform.localPosition = boat.Position.ToUnityVector3();
            _sailTransform.RotateOnAxis(Axis.Y, boat.SailAngle);
            _sailTransform.ScaleOnAxis(Axis.Z, _sailBow);
        }
    }
}
