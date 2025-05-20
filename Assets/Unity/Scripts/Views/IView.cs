using Sources.States;

namespace Unity.Scripts.Views {
    public interface IView {
        void Render(in GameState gameState, float dt);
    }
}
