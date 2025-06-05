using Sources;
using Unity.Scripts.Views;
using UnityEngine;

namespace Unity.Scripts {
    public class SceneBehaviour : MonoBehaviour {
        [SerializeField] private GameControllerBehaviour _gameControllerScript;
        [SerializeField] private SceneType _sceneType;

        public static GameControllerBehaviour GameControllerInstance;
        
        private AbstractView[] _views;
        
        public SceneType SceneType => _sceneType;

        private void Awake() {
            if (!GameControllerInstance) {
                GameControllerInstance = Instantiate(_gameControllerScript);
                GameControllerInstance.gameObject.name = "GameController";
                GameControllerInstance.Init(_sceneType);
            }
            
            GameControllerInstance.CurrentScene = this;

            _views = FindObjectsByType<AbstractView>(FindObjectsSortMode.None);
        }

        public void Render(GameState gameState, float dt) {
            foreach (var view in _views) {
                view.Render(gameState, dt);
            }
        }
    }
}
