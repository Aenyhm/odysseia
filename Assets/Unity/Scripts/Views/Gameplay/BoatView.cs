using Sources;
using Sources.Toolbox;
using UnityEngine;
using PlayMode = Sources.PlayMode;

namespace Unity.Scripts.Views.Gameplay {
    public class BoatView : AbstractView {
        private const float _minSailBow = 0.35f;
        private const float _maxSailBow = 0.75f;
        private const float _gameOverMoveSpeed = 2f;

        [SerializeField] private Transform _sailTransform;
        [SerializeField] private AudioSource _audio;

        private float _sailBow = _minSailBow;
        private float _gameOverRotateSpeed = 10f;
        private byte _lastHealth;
        
        void Awake() {
            _audio = GetComponent<AudioSource>();
        }
        
        void Start() {
            _lastHealth = Services.Get<GameConf>().BoatConf.HealthMax;
        }
        
        public override void Render(GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;
            
            var targetSailBow = boat.SailWindward ? _maxSailBow : _minSailBow;
            _sailBow = Maths.MoveTowards(_sailBow, targetSailBow, dt);
            _sailTransform.RotateOnAxis(Axis.Y, boat.SailAngle);
            _sailTransform.ScaleOnAxis(Axis.Z, _sailBow);
            
            if (_lastHealth != boat.Health) {
                _lastHealth = boat.Health;
                _audio.Play();
            }
            
            if (gameState.PlayState.Mode == PlayMode.Play) {
                // Pour que le déplacement latéral ne soit pas saccadé, on interpole avec le frame delta time.
                var speedX = Services.Get<GameConf>().BoatConf.SpeedX;
                var x = Maths.MoveTowards(transform.localPosition.x, boat.Position.X, dt*speedX);
                transform.MoveOnAxis(Axis.X, x);
                transform.MoveOnAxis(Axis.Z, boat.Position.Z);
            } else if (gameState.PlayState.Mode == PlayMode.GameOver) {
                transform.Translate(0, 0, _gameOverMoveSpeed*dt);
                transform.Rotate(_gameOverRotateSpeed*dt, 0, 0);
                _gameOverRotateSpeed -= dt;
            }
        }
    }
}
