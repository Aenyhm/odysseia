using Sources;
using Sources.Toolbox;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class HealthView : AbstractView {
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private Color _colorEnabled;
        [SerializeField] private Color _colorDisabled;

        private Image[] _heartImages;

        private void Start() {
            var healthMax = Services.Get<GameConf>().BoatConf.HealthMax;
            
            _heartImages = new Image[healthMax];
            
            for (var i = 0; i < healthMax; i++) {
                var go = Instantiate(_heartPrefab, transform);
                _heartImages[i] = go.GetComponent<Image>();
            }
        }

        public override void Render(GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;
            
            for (var i = 0; i < _heartImages.Length; i++) {
                _heartImages[i].color = boat.Health > i ? _colorEnabled : _colorDisabled;
            }
        }
    }
}
