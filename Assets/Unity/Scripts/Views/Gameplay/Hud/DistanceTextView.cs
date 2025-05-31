using System;
using Sources;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class DistanceTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            var distance = (int)gameState.PlayState.Boat.Distance;
            
            _textComponent.text = $"{distance} m";
        }
    }
}
