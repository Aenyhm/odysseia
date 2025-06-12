using Sources;
using Unity.Scripts.Views;
using UnityEngine;

namespace Unity.Scripts {
    // A attacher sur chaque scène. La première instancie le GameControllerBehaviour
    // pour pouvoir lancer le jeu depuis n'importe laquelle dans l'éditeur.
    public class SceneBehaviour : MonoBehaviour {
        [SerializeField] private GameControllerBehaviour _gameControllerScript;
        [SerializeField] private SceneType _sceneType;
        
        private AbstractView[] _views;
        
        public SceneType SceneType => _sceneType;

        private void Awake() {
            if (!GameControllerBehaviour.Instance) {
                Instantiate(_gameControllerScript);
                GameControllerBehaviour.Instance.Init(_sceneType);
            }
            
            GameControllerBehaviour.Instance.CurrentScene = this;

            _views = FindObjectsByType<AbstractView>(FindObjectsSortMode.None);
        }

        public void Render(GameState gameState, float dt) {
            foreach (var view in _views) {
                view.Render(gameState, dt);
            }
        }
    }
}
