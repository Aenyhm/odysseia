using System.Collections;
using Sources;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Scripts.Buttons {
    public class ChangeSceneButtonBehaviour : MonoBehaviour {
        [SerializeField] private SceneType _sceneType;
        [SerializeField] private PlayerActionType _actionType;
        
        void Update() {
            if (UnityInput.GetAction(_actionType)) OnClick();
        }

        public void OnClick() {
            StartCoroutine(Coroutine());
        }

        IEnumerator Coroutine() {
            var audioSource = GetComponent<AudioSource>();
            if (audioSource) {
                audioSource.Play();
                
                yield return new WaitForSeconds(audioSource.clip.length);
            }
            
            SceneManager.LoadScene((int)_sceneType);
        }
    }
}
