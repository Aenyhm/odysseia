using System;
using System.Collections;
using Sources.States;
using Sources.Toolbox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity.Scripts.Views.Gameplay {
    public class GameOverBackgroundView : AbstractView {
        private const float _gameOverSpeed = 0.2f;
        private const float _endWait = 1f;

        private Image _image;

        private void Awake() {
            _image = GetComponent<Image>();
        }
        
        public override void Render(in GameState gameState, float dt) {
            if (gameState.GameMode == GameMode.GameOver) {
                var targetAlpha = Maths.MoveTowards(_image.color.a, 1f, dt*_gameOverSpeed);
                if (Math.Abs(targetAlpha - 1) < float.Epsilon) {
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
