using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public class HudView : MonoBehaviour {
        [SerializeField] private RectTransform _healthBar;
        private float fullWidth;
        
        private void Start() {
            fullWidth = _healthBar.sizeDelta.x;
        }

        private void Update() {
            var boat = Services.Get<GameState>().boat;
            if (boat.health >= 0) {
                var size = _healthBar.sizeDelta;
                size.x = (float)boat.health/BoatController.HEALTH_MAX*fullWidth;
                _healthBar.sizeDelta = size;
            }
        }
    }
}
