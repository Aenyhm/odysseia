using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CannonballManagerView : AbstractManagerView<Cannonball> {
        private CannonballManagerView() : base("Cannonball") { }
        
        protected override int GetId(Cannonball data) => data.Id;

        protected override ICollection<Cannonball> GetElements(GameState gameState) {
            return gameState.PlayState.Cannonballs.ToList();
        }

        protected override void InitChild(GameObject go, Cannonball data) {
            go.GetComponent<AudioSource>().Play();
        }

        protected override void UpdateChild(GameObject go, Cannonball data) {
            go.transform.localPosition = data.Position.ToUnityVector3();
        }
    }
}
