using System.Collections.Generic;
using Sources;

namespace Unity.Scripts.Views.Gameplay {
    public class MermaidManagerView : AbstractEntityManagerView {
        protected override EntityType Type => EntityType.Mermaid;

        public override void Render(in GameState gameState, float dt) {
            var views = new List<EntityView>();
            
            foreach (var e in gameState.PlayState.Region.Entities) {
                if (e.Type == EntityType.Mermaid) {
                    var entityView = new EntityView();
                    entityView.Id = e.Id;
                    entityView.Position = e.Position.ToUnityVector3();
                    views.Add(entityView);
                }
            }
            
            Sync(views, dt);
        }
    }
}
