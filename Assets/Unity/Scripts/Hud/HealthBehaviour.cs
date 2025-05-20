using Sources;
using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class HealthBehaviour : MonoBehaviour, IViewRenderer {
        private RectTransform _rectTransform;
        private float _fullWidth;
        
        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _fullWidth = _rectTransform.sizeDelta.x;
        }

        public void Render(in GameState gameState, float dt) {
            var boat = gameState.Boat;
            
            var healthRatio = (float)boat.Health.Value/boat.Health.Max;
            var size = _rectTransform.sizeDelta;
            size.x = healthRatio*_fullWidth;
            _rectTransform.sizeDelta = size;
        }
    }
}
