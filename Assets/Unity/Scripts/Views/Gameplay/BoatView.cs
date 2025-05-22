using Sources.States;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class BoatView : AbstractView {
        private const float _minSailBow = 0.35f;
        private const float _maxSailBow = 0.75f;
        private const float _gameOverMoveSpeed = 2f;
        private float _gameOverRotateSpeed = 10f;

        [SerializeField] private Transform _sailTransform;
        
        private float _sailBow = _minSailBow;

        public override void Render(in GameState gameState, float dt) {
            var boat = gameState.Boat;
            
            var targetSailBow = boat.SailWindward ? _maxSailBow : _minSailBow;
            _sailBow = Maths.MoveTowards(_sailBow, targetSailBow, dt);
            _sailTransform.RotateOnAxis(Axis.Y, boat.SailAngle);
            _sailTransform.ScaleOnAxis(Axis.Z, _sailBow);
            
            if (gameState.GameMode == GameMode.Run) {
                transform.localPosition = boat.Position.ToUnityVector3();
            } else if (gameState.GameMode == GameMode.GameOver) {
                transform.Translate(0, 0, _gameOverMoveSpeed*dt);
                transform.Rotate(_gameOverRotateSpeed*dt, 0, 0);
                _gameOverRotateSpeed -= dt;
            }
        }
    }
}
