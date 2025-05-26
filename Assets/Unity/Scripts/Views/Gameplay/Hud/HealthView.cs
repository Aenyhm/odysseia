using Sources;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class HealthView : AbstractView {
        private RectTransform _rectTransform;
        private float _fullWidth;
        
        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _fullWidth = _rectTransform.sizeDelta.x;
        }

        public override void Render(in GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;
            var healthMax = Services.Get<GameConf>().BoatConf.HealthMax;
            
            var healthRatio = (float)boat.Health/healthMax;
            var size = _rectTransform.sizeDelta;
            size.x = healthRatio*_fullWidth;
            _rectTransform.sizeDelta = size;
        }
    }
}
