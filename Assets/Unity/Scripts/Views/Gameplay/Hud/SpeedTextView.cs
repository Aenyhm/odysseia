using Sources;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class SpeedTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            var speedZ = (int)gameState.PlayState.Boat.SpeedZ;
            
            _textComponent.text = $"{speedZ} m/s";
        }
    }
}
