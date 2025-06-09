using Sources;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts.Views.Gameplay {
    public class ControlsView : AbstractView {
        [SerializeField] private Sprite _keyboardSprite;
        [SerializeField] private Sprite _gamepadSprite;
        
        private Image _image;
        
        private void Awake() {
            _image = GetComponent<Image>();
        }
        
        public override void Render(GameState gameState, float dt) {
            _image.enabled = GameControllerBehaviour.Instance.ShowControls;
            _image.sprite = gameState.PlayerActions.UsingGamepad ? _gamepadSprite : _keyboardSprite;
        }
    }
}
