using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CannonballManagerView : AbstractManagerView<Cannonball> {
        private CannonballManagerView() : base("Cannonball") { }

        public override void Render(GameState gameState, float dt) {
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
            go.GetComponent<AudioSource>().Play();
        }
    }
}
