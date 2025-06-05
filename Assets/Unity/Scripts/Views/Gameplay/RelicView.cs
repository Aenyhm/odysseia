using Sources;
using Sources.Core;

namespace Unity.Scripts.Views.Gameplay {
    public class RelicView : AbstractView {
        public override void Render(GameState gameState, float dt) {
            var data = gameState.PlayState.Region.Entities.First(e => e.Type == EntityType.Relic);
            
            transform.position = data.Position.ToUnityVector3();
        }
    }
}
