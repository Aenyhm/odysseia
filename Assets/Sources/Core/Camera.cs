using Sources.Toolbox;

namespace Sources.Core {
    public class Camera3D : Entity { }
    
    public static class CameraController {
        public static Camera3D Create() {
            var camera = EntityManager.Create<Camera3D>();
            camera.transform.position.y = 8;
            camera.transform.position.z = -10;
            camera.transform.rotation.x = 20;
            
            Services.Get<IPlatform>().AddEntityView(camera);
            
            return camera;
        }
        
        public static void Update() {
            var gs = Services.Get<GameState>();
            gs.camera.transform.position.z = gs.boat.transform.position.z - 10;
        }
    }
}
