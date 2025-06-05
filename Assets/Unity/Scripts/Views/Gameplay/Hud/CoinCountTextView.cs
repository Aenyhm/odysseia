using Sources;
using TMPro;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CoinCountTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        private AudioSource _audioSource;
        private int _previousCount;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        public override void Render(GameState gameState, float dt) {
            var coinCount = gameState.PlayState.CoinCount;
            
            _textComponent.text = coinCount.ToString();
            
            if (coinCount != _previousCount) {
                _previousCount = coinCount;
                _audioSource.PlayOneShot(_audioSource.clip, 0.5f);
            }
        }
    }
}
