using Sources.View;
using UnityEngine;

namespace Unity.Scripts.Hud {
    public class HealthBehaviour : MonoBehaviour, IViewRenderer {
        private RectTransform _rectTransform;
        private float _fullWidth;
        
        private void Start() {
            _rectTransform = GetComponent<RectTransform>();
            _fullWidth = _rectTransform.sizeDelta.x;
        }

        public void Render(in ViewState viewState) {
            if (viewState.BoatView.HealthRatio >= 0) {
                var size = _rectTransform.sizeDelta;
                size.x = viewState.BoatView.HealthRatio*_fullWidth;
                _rectTransform.sizeDelta = size;
            }
        }
    }
}
