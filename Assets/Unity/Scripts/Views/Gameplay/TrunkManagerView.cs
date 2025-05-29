using System.Collections.Generic;
using Sources;
using Sources.Core;

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
                var pos = EntityLogic.GetPosition(e.Type, e.Coords);
                
                go.transform.localPosition = pos.ToUnityVector3();
            }
        }
    }
}
