using Sources.View;

namespace Unity.Scripts {
    public interface IViewRenderer {
        void Render(in ViewState viewState);
    }
}
