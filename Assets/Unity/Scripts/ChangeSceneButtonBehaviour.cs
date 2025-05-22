using Sources.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Scripts {
    public class ChangeSceneButtonBehaviour : MonoBehaviour {
        [SerializeField] private SceneType _sceneType;
        
        public void OnClick() {
            SceneManager.LoadScene((int)_sceneType);
        }
    }
}
