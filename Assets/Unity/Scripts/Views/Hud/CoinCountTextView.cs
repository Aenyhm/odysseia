using Sources.States;
using TMPro;
using UnityEngine;

namespace Unity.Scripts.Views.Hud {
    public class CoinCountTextView : MonoBehaviour, IView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public void Render(in GameState gameState, float dt) {
            _textComponent.text = gameState.CoinCount.ToString();
        }
    }
}
