using Sources.States;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class RunCoinCountTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            _textComponent.text = gameState.RunCoinCount.ToString();
        }
    }
}
