using System.Collections.Generic;
using Sources;
using Sources.States;

namespace Unity.Scripts {
    public class RockManagerBehaviour : EntityManagerBehaviour, IViewRenderer {
        public void Render(in GameState gameState, float dt) {
            var views = new List<EntityView>();
            
            foreach (var obstacle in gameState.Region.Obstacles) {
                if (obstacle.Type == EntityType.Rock) {
                    var entityView = new EntityView();
                    entityView.Id = obstacle.Id;
                    entityView.Type = obstacle.Type;
                    entityView.Position = obstacle.Position.ToUnityVector3();
                    views.Add(entityView);
                }
            }
            
            Sync(views);
        }
    }
}
