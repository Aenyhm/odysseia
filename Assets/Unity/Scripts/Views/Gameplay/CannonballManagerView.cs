using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CannonballManagerView : AbstractManagerView<Cannonball> {
        private CannonballManagerView() : base("Ammo") { }

        public override void Render(in GameState gameState, float dt) {
            var entities = gameState.PlayState.Cannonballs;
            
            var entitiesById = new Dictionary<int, Cannonball>(entities.Count);
            foreach (var e in entities) {
                entitiesById.Add(e.Id, e);
            }
            
            Sync(entitiesById);
            
            foreach (var (id, go) in _gosById) {
                var e = entitiesById[id];
                go.transform.localPosition = e.Position.ToUnityVector3();
            }
        }

        protected override void InitChild(GameObject go, Cannonball data) {
            if (!data.Lootable) {
                go.GetComponent<AudioSource>().Play();
            }
        }
    }
}
