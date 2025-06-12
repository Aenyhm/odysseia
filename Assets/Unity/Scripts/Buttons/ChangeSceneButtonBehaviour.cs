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
                
                // On laisse un court délai entre le clic du bouton et le changement de scène
                // le temps que le son se joue.
                yield return new WaitForSeconds(audioSource.clip.length);
            }
            
            SceneManager.LoadScene((int)_sceneType);
        }
    }
}
