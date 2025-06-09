using Sources.Core;
using UnityEngine;

namespace Unity.Scripts {
    public class CoinBonusBehaviour : MonoBehaviour {
        private AudioSource _audio;

        void Awake() {
            _audio = GetComponent<AudioSource>();
        }
        
        void OnEnable() {
            CoinSystem.LineCompleted += OnLineCompleted;
        }
        
        void OnDisable() {
            CoinSystem.LineCompleted -= OnLineCompleted;
        }
        
        private void OnLineCompleted() {
            _audio!.PlayOneShot(_audio.clip);
        }
    }
}
