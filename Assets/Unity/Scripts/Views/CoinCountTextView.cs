using Sources.States;
using TMPro;

namespace Unity.Scripts.Views {
    public class CoinCountTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            _textComponent.text = gameState.CoinCount.ToString();
        }
    }
}
