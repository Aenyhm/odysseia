using Sources;
using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class RegionView : AbstractView {
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _light;
        [SerializeField] private Renderer _waterRenderer;
        
        private RegionType? _regionType;
        
        public override void Render(in GameState gameState, float dt) {
            if (_regionType == null || _regionType != gameState.Region.Type) {
                _regionType = gameState.Region.Type;
                
                var regionTheme = ViewConfig.regionThemesByType[gameState.Region.Type];
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
