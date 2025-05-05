using Sources.Toolbox;

namespace Sources.Core {
    public struct InputState {
        public float Horizontal;
    }

    public static class InputHandler {
        public static InputState Read() {
            var platform = Services.Get<Platform>();
            
            InputState input;
            input.Horizontal = platform.GetHorizontalAxisInput();
            return input;
        }
    }
}
