using Sources.Core;

namespace Sources {
    public interface IRenderer {
        void Create(Entity entity);
        void Destroy(Entity entity);
        void Update();
    }
    
    public abstract class Platform {
        public readonly IRenderer renderer;

        protected Platform(IRenderer renderer) {
            this.renderer = renderer;
        }
        
        public abstract void Log(string message);
        public abstract float GetHorizontalAxisInput();
    }
}
