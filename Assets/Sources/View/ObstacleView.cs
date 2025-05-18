using System.Collections.Generic;
using Sources.Core;

namespace Sources.View {
    public static class ObstacleViewSystem {
        public static (List<EntityView> rockViews, List<EntityView> trunkViews) Update(IEnumerable<Obstacle> obstacles) {
            var rockViews = new List<EntityView>();
            var trunkViews = new List<EntityView>();

            foreach (var obstacle in obstacles) {
                var entityView = new EntityView();
                entityView.Id = obstacle.Id;
                entityView.Type = obstacle.Type;
                entityView.Position = obstacle.Position;
                
                switch (obstacle.Type) {
                    case EntityType.Rock:
                        rockViews.Add(entityView);
                        break;
                    case EntityType.Trunk:
                        trunkViews.Add(entityView);
                        break;
                }
            }
            
            return (rockViews, trunkViews);
        }
    }
}
