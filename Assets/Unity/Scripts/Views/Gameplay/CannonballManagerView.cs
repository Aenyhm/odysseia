using System.Collections.Generic;
using Sources;
using Sources.Core;

namespace Unity.Scripts.Views.Gameplay {
    public class CannonballManagerView : AbstractEntityManagerView {
        protected override EntityType Type => EntityType.Cannonball;

        public override void Render(in GameState gameState, float dt) {
            var entities = gameState.PlayState.Entities.Cannonballs;
            
            var entitiesById = new Dictionary<int, Cannonball>(entities.Count);
            for (var i = 0; i < entities.Count; i++) {
                var e = entities.Items[i];
                entitiesById.Add(e.Id, e);
            }
            
            Sync(entitiesById.Keys);
            
            foreach (var (id, go) in _gosById) {
                var coin = entitiesById[id];
                go.transform.localPosition = coin.Position.ToUnityVector3();
            }
        }
    }
}
