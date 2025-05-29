using Sources;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CannonAmmoView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            var cannon = gameState.PlayState.Cannon;
            
            _textComponent.text = cannon.AmmoCount.ToString();
        }
    }
}
