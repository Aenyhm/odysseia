using Sources;
using TMPro;

namespace Unity.Scripts.Views.Title {
    public class TotalCoinCountTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            _textComponent.text = gameState.GlobalProgression.CoinCount.ToString();  // TODO: animation incrément
        }
    }
}
