using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;
using Sources.View;

namespace Sources {
    public struct CoreState {
        public Boat Boat;
        public Region Region;
        public Wind Wind;
    }
    
    public struct Conf {
        public Dictionary<EntityType, Vec3F32> Sizes;
    }
    
    public class Game {
        private CoreState _coreState;
        private ViewState _previousViewState;

        public Game(IPlatform platform, Conf conf) {
            Services.Register(platform);
            Services.Register(new Random());
            
            _coreState = new CoreState();
            
            BoatSystem.Init(in conf, out _coreState.Boat);
            RegionSystem.Init(in conf, out _coreState.Region);
            WindSystem.Init(out _coreState.Wind);
        }
        
        public void CoreUpdate(float dt, in GameInput input) {
            BoatSystem.Update(ref _coreState.Boat, in input, in _coreState.Wind, _coreState.Region.Obstacles, dt);
            RegionSystem.Update(ref _coreState.Region, _coreState.Boat.Position.Z);
            WindSystem.Update(ref _coreState.Wind, in _coreState.Boat, dt);
        }
        
        public void ViewUpdate(float dt, out ViewState viewState) {
            var (rockViews, trunkViews) = ObstacleViewSystem.Update(_coreState.Region.Obstacles);
            
            viewState = new ViewState {
                BoatView = BoatViewSystem.Update(in _previousViewState.BoatView, in _coreState.Boat, in _coreState.Wind, dt),
                CameraView = CameraViewSystem.Update(in _coreState.Boat),
                WindView = WindViewSystem.Update(in _coreState.Wind),
                RegionTheme = ViewConfig.regionThemesByType[_coreState.Region.Type],
                RockViews = rockViews,
                TrunkViews = trunkViews,
            };
            
            _previousViewState = viewState;
        }
    }
}
