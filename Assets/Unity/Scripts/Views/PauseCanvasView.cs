using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Views {
    public class PauseCanvasView : MonoBehaviour, IView {
        [SerializeField] private GameObject _pausePanel;
        
        public void Render(in GameState gameState, float dt) {
            _pausePanel.SetActive(gameState.Pause);
        }
    }
}
