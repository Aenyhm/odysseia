using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CameraView : AbstractView {
        private const float _defaultFov = 60f;
        private const float _gameOverSpeed = 5f;
        private Camera _camera;
        private Vector3 _initialPosition;
        private Vector3 _lastPosition;
        
        [SerializeField] private int _cameraSpeed = 4;

        private void Awake() {
            _camera = GetComponent<Camera>();
            _camera.fieldOfView = _defaultFov;
            
            _initialPosition = transform.localPosition;
            _lastPosition = _initialPosition;
        }
        
        public override void Render(in GameState gameState, float dt) {
            var boat = gameState.Boat;
            
            if (boat.Position.Z < _lastPosition.z) {
                transform.localPosition = _initialPosition;
            } else {
                var targetPosition = _lastPosition;
                targetPosition.z = boat.Position.Z + _initialPosition.z - boat.SpeedZ/_cameraSpeed;
                
                var velocity = boat.SpeedZ*_cameraSpeed*dt;
                transform.localPosition = Vector3.MoveTowards(_lastPosition, targetPosition, velocity);
            }
            
            _lastPosition = transform.localPosition;
            
            if (gameState.GameMode == GameMode.GameOver) {
                _camera.fieldOfView -= dt*_gameOverSpeed;
            }
        }
    }
}
