using Sources.Core;

namespace Sources.View {
    public struct WindView {
        public float Angle;
    }
    
    public static class WindViewSystem {
        public static WindView Update(in Wind wind) {
            var result = new WindView();
            result.Angle = wind.Angle;
            return result;
        }
    }
}
