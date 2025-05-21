using System;
using Sources.States;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class DistanceTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            var boat = gameState.Boat;
            var distance = Convert.ToInt64(boat.Distance);
            
            _textComponent.text = $"{distance} m";
        }
    }
}
