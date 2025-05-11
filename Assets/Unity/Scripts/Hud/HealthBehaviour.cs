using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class HealthBehaviour : MonoBehaviour {
        private RectTransform _rectTransform;
        private float _fullWidth;
        
        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _fullWidth = _rectTransform.sizeDelta.x;
        }

        private void Update() {
            var gs = Services.Get<GameState>();
            
            if (gs.boat.health >= 0) {
                var size = _rectTransform.sizeDelta;
                size.x = (float)gs.boat.health/BoatController.HEALTH_MAX*_fullWidth;
                _rectTransform.sizeDelta = size;
            }
        }
    }
}
