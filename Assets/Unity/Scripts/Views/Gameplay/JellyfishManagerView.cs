using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class JellyfishManagerView : AbstractManagerView<Entity> {
        private JellyfishManagerView() : base("Jellyfish") { }
        
        public override void Render(in GameState gameState, float dt) {
            var entitiesById = new Dictionary<int, Entity>();
            foreach (var e in gameState.PlayState.Region.Entities) {
                if (e.Type == EntityType.Jellyfish) {
                    entitiesById[e.Id] = e;
                }
            }
            
            Sync(entitiesById);
            
            foreach (var (id, go) in _gosById) {
                var e = entitiesById[id];
                go.transform.localPosition = e.Position.ToUnityVector3();
            }
        }

        protected override void InitChild(GameObject go, Entity data) {
        }
    }
}
