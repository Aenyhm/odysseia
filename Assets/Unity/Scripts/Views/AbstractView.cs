using Sources;
using UnityEngine;

namespace Unity.Scripts.Views {
    // Tous les composants héritant de cette classe se voit passer
    // l'état du jeu lors de l'Update du GameControllerBehaviour.
    public abstract class AbstractView : MonoBehaviour {
        public abstract void Render(GameState gameState, float dt);
    }
}
