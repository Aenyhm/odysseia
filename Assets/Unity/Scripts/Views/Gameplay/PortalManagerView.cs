using System;
using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    [Serializable]
    public struct RegionPortal {
        public RegionType RegionType;
        public GameObject GameObject;
    }
    
    public class PortalManagerView : AbstractView {
        [SerializeField] private RegionPortal[] _regionPortals;
        
        private readonly Dictionary<RegionType, GameObject> _gosByRegionType = new();
        
        private void Awake() {
            foreach (var regionPortal in _regionPortals) {
                _gosByRegionType[regionPortal.RegionType] = regionPortal.GameObject;
                regionPortal.GameObject.SetActive(false);
            }
        }
        
        public override void Render(GameState gameState, float dt) {
            foreach (var (regionType, go) in _gosByRegionType) {
                var found = false;
                foreach (var portal in gameState.PlayState.Region.Portals) {
                    if (regionType == portal.RegionType) {
                        found = true;
                        var posX = LaneLogic.GetPosition((int)portal.LaneType);
                        go.transform.MoveOnAxis(Axis.X, posX);
                        break;
                    }
                }
                
                go.SetActive(found);
            }
        }
    }
}
