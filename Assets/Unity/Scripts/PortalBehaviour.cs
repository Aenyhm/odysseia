using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.View;
using UnityEngine;

namespace Unity.Scripts {
    [Serializable]
    public struct RegionCover {
        public RegionType RegionType;
        public Texture Texture;
    }
    
    public class PortalBehaviour : MonoBehaviour, IViewRenderer {
        [SerializeField] private RegionCover[] _regionCovers;
        
        private Renderer _renderer;
        private readonly Dictionary<RegionType, Texture> _coversByRegionType = new();
        
        private void Awake() {
            _renderer = GetComponent<Renderer>();
            
            foreach (var regionCover in _regionCovers) {
                _coversByRegionType[regionCover.RegionType] = regionCover.Texture;
            }
        }
        
        public void Render(in ViewState viewState) {
            var portalView = viewState.PortalView;
            
            transform.localPosition = portalView.Position.ToUnityVector3();
            
            var mat = _renderer.material;
            mat.mainTexture = _coversByRegionType[portalView.RegionType];
        }
    }
}
