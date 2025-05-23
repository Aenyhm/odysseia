using Sources;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CoinCountTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            _textComponent.text = gameState.PlayState.CoinCount.ToString();
        }
    }
}
