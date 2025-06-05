using Sources;
using UnityEngine;

namespace Unity.Scripts.Views {
    public abstract class AbstractView : MonoBehaviour {
        public abstract void Render(GameState gameState, float dt);
    }
}
