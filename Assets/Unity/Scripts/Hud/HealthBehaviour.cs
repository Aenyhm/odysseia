using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class HealthBehaviour : MonoBehaviour {
        private RectTransform _rectTransform;
        private float _healthBarFullWidth;
        
        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _healthBarFullWidth = _rectTransform.sizeDelta.x;
        }

        private void Update() {
            var gs = Services.Get<GameState>();
            
            if (gs.boat.health >= 0) {
                var size = _rectTransform.sizeDelta;
                size.x = (float)gs.boat.health/BoatController.HEALTH_MAX*_healthBarFullWidth;
                _rectTransform.sizeDelta = size;
            }
        }
    }
}
