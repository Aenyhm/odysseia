using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts {
    [RequireComponent(typeof(Slider))]
    public class VolumeSliderBehaviour : MonoBehaviour {
        private Slider _slider;
        
        void Awake() {
            _slider = GetComponent<Slider>();
        }
        
        void OnEnable() {
            _slider.value = PlayerPrefs.GetFloat("AudioVolume")*_slider.maxValue;
        }
        
        public void OnValueChanged() {
            AudioListener.volume = _slider.value/_slider.maxValue;
            PlayerPrefs.SetFloat("AudioVolume", AudioListener.volume);
        }
    }
}
