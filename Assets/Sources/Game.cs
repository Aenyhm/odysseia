using System;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    public class GameState {
        public Camera3D camera;
        public Boat boat;
        public Region region;
        public Wind wind;
    }
    
    public class Game {
        public Game(IPlatform platform) {
            Services.Register(platform);
            Services.Register(new Random());
            Services.Register(new UiConfig());

            var gs = new GameState();
            gs.camera = CameraController.Create();
            gs.boat = BoatController.Create();
            gs.region = new Region();
            gs.region.type = (RegionType)Services.Get<Random>().Next(5); //RegionType.Aegis;
            gs.wind = WindController.Create();
            
            Services.Register(gs);
        }
        
        public void Update(float dt, GameInput input) {
            BoatController.Update(dt, input);
            CameraController.Update();
            SceneryController.Update();
            WindController.Update(dt);
        }
    }
}
