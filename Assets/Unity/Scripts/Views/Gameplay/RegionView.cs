using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class RegionView : AbstractView {
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _light;
        [SerializeField] private Renderer _waterRenderer;
        
        private RegionType? _regionType;
        
        public override void Render(GameState gameState, float dt) {
            var region = gameState.PlayState.Region;
            
            if (_regionType == null || _regionType != region.Type) {
                _regionType = region.Type;
                
                var regionTheme = ViewConfig.regionThemesByType[region.Type];
                ApplyRegionTheme(regionTheme);
            }
        }
        
        private void ApplyRegionTheme(RegionTheme theme) {
            RenderSettings.fogDensity = ViewConfig.FOG_DENSITY;
            RenderSettings.fogColor = theme.SkyColor.ToUnityColor();
            
            _camera.backgroundColor = theme.SkyColor.ToUnityColor();
            
            _light.color = theme.LightColor.ToUnityColor();
            _light.intensity = theme.LightIntensity;

            var waterMaterial = _waterRenderer.material;
            waterMaterial.color = theme.WaterColor.ToUnityColor();
        }
    }
}
