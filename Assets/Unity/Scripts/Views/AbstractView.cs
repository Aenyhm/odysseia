using Sources;
using UnityEngine;

namespace Unity.Scripts.Views {
    public abstract class AbstractView : MonoBehaviour {
        public abstract void Render(in GameState gameState, float dt);
    }
}
