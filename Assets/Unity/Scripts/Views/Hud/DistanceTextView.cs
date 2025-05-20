using System;
using Sources.States;
using TMPro;
using UnityEngine;

namespace Unity.Scripts.Views.Hud {
    public class DistanceTextView : MonoBehaviour, IView {
        private TextMeshProUGUI _textComponent;
        
        private void Awake() {
            _textComponent = GetComponent<TextMeshProUGUI>();
        }
        
        public void Render(in GameState gameState, float dt) {
            var boat = gameState.Boat;
            var distance = Convert.ToInt64(boat.Distance);
            
            _textComponent.text = $"{distance} m";
        }
    }
}
