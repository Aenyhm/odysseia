using Sources;
using UnityEngine;
using PlayMode = Sources.PlayMode;

namespace Unity.Scripts.Views.Gameplay {
    public class PauseCanvasView : AbstractView {
        [SerializeField] private GameObject _pausePanel;
        
        public override void Render(in GameState gameState, float dt) {
            _pausePanel.SetActive(gameState.PlayState.Mode == PlayMode.Pause);
        }
    }
}
