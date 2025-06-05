using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class AmmoManagerView : AbstractManagerView<Cannonball> {
        private AmmoManagerView() : base("Ammo") { }

        public override void Render(GameState gameState, float dt) {
            var entities = gameState.PlayState.Ammos;
            
            var entitiesById = new Dictionary<int, Cannonball>(entities.Count);
            foreach (var e in entities) {
                entitiesById.Add(e.Id, e);
            }
            
            Sync(entitiesById);
        }

        protected override void InitChild(GameObject go, Cannonball data) {
            go.transform.localPosition = data.Position.ToUnityVector3();
        }
    }
}
