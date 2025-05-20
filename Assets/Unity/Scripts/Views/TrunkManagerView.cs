using System.Collections.Generic;
using Sources;
using Sources.States;

namespace Unity.Scripts.Views {
    public class TrunkManagerView : EntityManagerBehaviour, IView {
        public void Render(in GameState gameState, float dt) {
            var views = new List<EntityView>();
            
            foreach (var obstacle in gameState.Region.ObstaclesByType[EntityType.Trunk]) {
                var entityView = new EntityView();
                entityView.Id = obstacle.Id;
                entityView.Type = EntityType.Trunk;
                entityView.Position = obstacle.Position.ToUnityVector3();
                views.Add(entityView);
            }
            
            Sync(views);
        }
    }
}
