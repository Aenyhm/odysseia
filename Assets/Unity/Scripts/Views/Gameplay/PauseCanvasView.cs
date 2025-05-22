using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class PauseCanvasView : AbstractView {
        [SerializeField] private GameObject _pausePanel;
        
        public override void Render(in GameState gameState, float dt) {
            _pausePanel.SetActive(gameState.GameMode == GameMode.Pause);
        }
    }
}
