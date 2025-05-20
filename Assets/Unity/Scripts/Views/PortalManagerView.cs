using System;
using System.Collections.Generic;
using Sources.Mechanics;
using Sources.States;
using UnityEngine;

namespace Unity.Scripts.Views {
    [Serializable]
    public struct RegionPortal {
        public RegionType RegionType;
        public GameObject GameObject;
    }
    
    public class PortalManagerView : MonoBehaviour, IView {
        [SerializeField] private RegionPortal[] _regionPortals;
        
        private readonly Dictionary<RegionType, GameObject> _gosByRegionType = new();
        
        private void Awake() {
            foreach (var regionPortal in _regionPortals) {
                _gosByRegionType[regionPortal.RegionType] = regionPortal.GameObject;
                regionPortal.GameObject.SetActive(false);
            }
        }
        
        public void Render(in GameState gameState, float dt) {
            foreach (var (regionType, go) in _gosByRegionType) {
                var found = false;
                foreach (var portal in gameState.Region.Portals) {
                    if (regionType == portal.RegionType) {
                        found = true;
                        var posX = LaneMechanics.GetPosition(portal.LaneType, CoreConfig.LaneDistance);
                        go.transform.MoveOnAxis(Axis.X, posX);
                        break;
                    }
                }
                
                go.SetActive(found);
            }
        }
    }
}
