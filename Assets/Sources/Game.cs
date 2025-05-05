using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    public class GameState {
        public Camera3D camera;
        public Boat boat;
        public Scenery scenery;
    }
    
    public class Game {
        public Game(Platform platform) {
            Services.Register(platform);
            
            var gs = new GameState();
            gs.camera = CameraController.Create();
            gs.boat = BoatController.Create();
            gs.scenery = new Scenery();
            
            Services.Register(gs);
        }

        public void FixedUpdate(float dt) {
            
        }

        public void Update(float dt) {
            var inputState = InputHandler.Read();
            BoatController.Update(inputState, dt);
            CameraController.Update();
            SceneryController.Update();
            Services.Get<Platform>().renderer.Update();
        }
    }
}
