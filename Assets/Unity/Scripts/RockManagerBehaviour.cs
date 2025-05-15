using Sources.View;

namespace Unity.Scripts {
    public class RockManagerBehaviour : EntityManagerBehaviour, IViewRenderer {
        public void Render(in ViewState viewState) {
            Sync(viewState.RockViews);
        }
    }
}
