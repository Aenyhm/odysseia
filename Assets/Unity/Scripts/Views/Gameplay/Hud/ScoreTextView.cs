using Sources;
using TMPro;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class ScoreTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public override void Render(GameState gameState, float dt) {
            _textComponent.text = gameState.PlayState.PlayProgression.Score.ToString();
        }
    }
}
