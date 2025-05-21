using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Scripts {
    public class ChangeSceneButtonBehaviour : MonoBehaviour {
        [SerializeField] private int _sceneIndex;
        
        public void OnClick() {
            SceneManager.LoadScene(_sceneIndex);
        }
    }
}
