using System.Collections;
using Sources;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Scripts.Buttons {
    public class ChangeSceneButtonBehaviour : AbstractButton {
        [SerializeField] private SceneType _sceneType;
        
        protected override void DoAction() {
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
