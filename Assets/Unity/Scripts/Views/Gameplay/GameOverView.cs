using System.Collections;
using Sources;
using Sources.Toolbox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayMode = Sources.PlayMode;

namespace Unity.Scripts.Views.Gameplay {
    public class GameOverView : AbstractView {
        private const float _boatSpeed = 0.2f;
        private const float _endWait = 1f;

        private Image _image;
        private AudioSource _audio;
        private bool firstPass;

        private void Awake() {
            _image = GetComponent<Image>();
            _audio = GetComponent<AudioSource>();
        }
        
        public override void Render(GameState gameState, float dt) {
            if (gameState.PlayState.Mode == PlayMode.GameOver) {
                if (!firstPass) {
                    firstPass = true;
                    _audio.PlayOneShot(_audio.clip);
                }

                var targetAlpha = Maths.MoveTowards(_image.color.a, 1f, dt*_boatSpeed);
                if (Maths.FloatEquals(targetAlpha, 1)) {
                    targetAlpha = 1;
                    StartCoroutine(EndOfRun());
                }
                
                var color = _image.color;
                color.a = targetAlpha;
                _image.color = color;
            }
        }
        
        private static IEnumerator EndOfRun() {
            yield return new WaitForSeconds(_endWait);
            SceneManager.LoadScene((int)SceneType.Title);
        }
    }
}
