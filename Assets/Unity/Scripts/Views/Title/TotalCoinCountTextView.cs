using Sources;
using Sources.Toolbox;
using TMPro;
using UnityEngine;
using Animation = Sources.Toolbox.Animation;

namespace Unity.Scripts.Views.Title {
    public class TotalCoinCountTextView : AbstractView {
        private TextMeshProUGUI _textComponent;
        private AudioSource _audioSource;
        private Animation _incrementAnim;
        private float _incrementValue;
        
        [SerializeField] private float _incrementDuration = 1f;

        private void Start() {
            _textComponent = GetComponent<TextMeshProUGUI>();
            _audioSource = GetComponent<AudioSource>();
            _incrementValue = GameControllerBehaviour.Instance.TitleCoinCount;
        }

        public override void Render(GameState gameState, float dt) {
            var totalCoinCount = gameState.GlobalProgression.CoinCount;
            
            if (_incrementAnim != null && !_incrementAnim.Completed) {
                _incrementValue = _incrementAnim.Update();
                GameControllerBehaviour.Instance.TitleCoinCount = (int)_incrementValue;
                _audioSource.PlayOneShot(_audioSource.clip, 0.3f);
            } else if ((int)_incrementValue != totalCoinCount) {
                _incrementAnim = new Animation(_incrementValue, totalCoinCount, _incrementDuration, Easings.InOutSine);
            }

            _textComponent.text = ((int)_incrementValue).ToString();
        }
    }
}
