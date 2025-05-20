using System.Collections.Generic;
using Sources;
using Sources.States;

namespace Unity.Scripts.Views {
    public class RockManagerView : EntityManagerBehaviour, IView {
        public void Render(in GameState gameState, float dt) {
            var views = new List<EntityView>();
            
            foreach (var obstacle in gameState.Region.ObstaclesByType[EntityType.Rock]) {
                var entityView = new EntityView();
                entityView.Id = obstacle.Id;
                entityView.Type = EntityType.Rock;
                entityView.Position = obstacle.Position.ToUnityVector3();
                views.Add(entityView);
            }
            
            Sync(views);
        }
    }
}
