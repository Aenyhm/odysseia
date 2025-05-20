using Sources.States;

namespace Unity.Scripts {
    public interface IViewRenderer {
        void Render(in GameState gameState, float dt);
    }
}
