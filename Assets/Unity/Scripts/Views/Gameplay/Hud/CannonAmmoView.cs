using Sources;
using TMPro;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CannonAmmoView : AbstractView {
        private TextMeshProUGUI _textComponent;
        private AudioSource _audioSource;
        private int _previousCount;

        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
            _audioSource = GetComponent<AudioSource>();
        }
        
        public override void Render(GameState gameState, float dt) {
            var ammoCount = gameState.PlayState.Cannon.AmmoCount;
            
            _textComponent.text = ammoCount.ToString();
            
            if (ammoCount > _previousCount) {
                _audioSource.PlayOneShot(_audioSource.clip, 0.5f);
            }

            _previousCount = ammoCount;
        }
    }
}
