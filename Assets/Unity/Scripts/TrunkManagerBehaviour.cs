using Sources.View;

namespace Unity.Scripts {
    public class TrunkManagerBehaviour : EntityManagerBehaviour, IViewRenderer {
        public void Render(in ViewState viewState) {
            Sync(viewState.TrunkViews);
        }
    }
}
