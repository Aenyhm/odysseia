using Sources.Core;

namespace Sources.View {
    public static class CameraViewSystem {
        public static EntityView Update(in Boat boat) {
            var result = new EntityView();
            result.Position.Z = boat.Position.Z - 10;
            return result;
        }
    }
}
