using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class AmmoManagerView : AbstractManagerView<Cannonball> {
        private AmmoManagerView() : base("Ammo") { }

        protected override int GetId(Cannonball data) => data.Id;

        protected override ICollection<Cannonball> GetElements(GameState gameState) {
            return gameState.PlayState.Ammos.ToList();
        }

        protected override void InitChild(GameObject go, Cannonball data) {
            go.transform.localPosition = data.Position.ToUnityVector3();
        }

        protected override void UpdateChild(GameObject go, Cannonball data) {
        }
    }
}
