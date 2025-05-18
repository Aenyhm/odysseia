using Sources.Core;

namespace Sources.View {
    public static class CameraViewSystem {
        private static Conf _conf;
        
        public static void Init(in Conf conf) {
            _conf = conf;
        }
        
        public static EntityView Update(in Boat boat) {
            var result = new EntityView();
            result.Position.Z = boat.Position.Z + _conf.CameraZ;
            return result;
        }
    }
}
