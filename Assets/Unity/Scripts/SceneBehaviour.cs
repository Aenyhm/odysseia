using System;
using Sources.States;
using Unity.Scripts.Views;
using UnityEngine;

namespace Unity.Scripts {
    public class SceneBehaviour : MonoBehaviour {
        [SerializeField] private GameControllerBehaviour _gameControllerScript;
        [SerializeField] private SceneType _sceneType;

        private static GameControllerBehaviour _gameControllerInstance;
        private AbstractView[] _views;
        
        public SceneType SceneType => _sceneType;

        private void Awake() {
            if (!_gameControllerInstance) {
                _gameControllerInstance = Instantiate(_gameControllerScript);
                _gameControllerInstance.gameObject.name = "GameController";
            }
            
            _gameControllerInstance.CurrentScene = this;

            _views = FindObjectsByType<AbstractView>(FindObjectsSortMode.None);
        }

        public void Render(in GameState gameState, float dt) {
            foreach (var view in _views) {
                view.Render(in gameState, dt);
            }
        }
    }
}
