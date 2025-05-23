using Sources;
using Sources.Toolbox;
using UnityEngine;
using PlayMode = Sources.PlayMode;

namespace Unity.Scripts.Views.Gameplay {
    public class BoatView : AbstractView {
        private const float _minSailBow = 0.35f;
        private const float _maxSailBow = 0.75f;
        private const float _gameOverMoveSpeed = 2f;
        private float _gameOverRotateSpeed = 10f;

        [SerializeField] private Transform _sailTransform;
        
        private float _sailBow = _minSailBow;

        public override void Render(in GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;
            
            var targetSailBow = boat.SailWindward ? _maxSailBow : _minSailBow;
            _sailBow = Maths.MoveTowards(_sailBow, targetSailBow, dt);
            _sailTransform.RotateOnAxis(Axis.Y, boat.SailAngle);
            _sailTransform.ScaleOnAxis(Axis.Z, _sailBow);
            
            if (gameState.PlayState.Mode == PlayMode.Play) {
                transform.localPosition = boat.Position.ToUnityVector3();
            } else if (gameState.PlayState.Mode == PlayMode.GameOver) {
                transform.Translate(0, 0, _gameOverMoveSpeed*dt);
                transform.Rotate(_gameOverRotateSpeed*dt, 0, 0);
                _gameOverRotateSpeed -= dt;
            }
        }
    }
}
