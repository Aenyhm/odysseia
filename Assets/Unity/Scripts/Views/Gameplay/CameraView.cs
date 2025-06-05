using Sources;
using UnityEngine;
using PlayMode = Sources.PlayMode;

namespace Unity.Scripts.Views.Gameplay {
    public class CameraView : AbstractView {
        private Camera _camera;
        private float _initialFov;
        private float _initialZ;
        
        [SerializeField] private float _gameOverSpeed = 5f;
        
        private void Awake() {
            _camera = GetComponent<Camera>();
            _initialFov = _camera.fieldOfView;
            _initialZ = transform.localPosition.z;
        }
        
        public override void Render(GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;
            
            if (gameState.PlayState.Mode != PlayMode.GameOver) {
                transform.MoveOnAxis(Axis.Z, boat.Position.Z + _initialZ);
                
                _camera.fieldOfView = _initialFov + boat.SpeedZ;
            } else {
                _camera.fieldOfView -= dt*_gameOverSpeed;
            }
        }
    }
}
