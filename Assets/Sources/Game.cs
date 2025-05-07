using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    public class GameState {
        public Camera3D camera;
        public Boat boat;
        public Scenery scenery;
    }
    
    public class Game : IGame {
        public Game(IPlatform platform) {
            Services.Register(platform);
            
            var gs = new GameState();
            gs.camera = CameraController.Create();
            gs.boat = BoatController.Create();
            gs.scenery = new Scenery();
            
            Services.Register(gs);
        }
        
        public void Update(float dt, GameInput input) {
            BoatController.Update(dt, input);
            CameraController.Update();
            SceneryController.Update();
        }
    }
}
