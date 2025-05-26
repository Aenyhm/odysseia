using System.Collections.Generic;
using Sources;

namespace Unity.Scripts.Views.Gameplay {
    public class TrunkManagerView : AbstractEntityManagerView {
        protected override EntityType Type => EntityType.Trunk;

        public override void Render(in GameState gameState, float dt) {
            var entitiesById = new Dictionary<int, Entity>();
            foreach (var e in gameState.PlayState.Region.Entities) {
                if (e.Type == EntityType.Trunk) {
                    entitiesById[e.Id] = e;
                }
            }
            
            Sync(entitiesById.Keys);
            
            foreach (var (id, go) in _gosById) {
                var e = entitiesById[id];
                go.transform.localPosition = e.Position.ToUnityVector3();
            }
        }
    }
}
