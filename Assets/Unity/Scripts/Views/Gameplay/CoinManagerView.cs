using System.Collections.Generic;
using Sources;

namespace Unity.Scripts.Views.Gameplay {
    public class CoinManagerView : AbstractEntityManagerView {
        public override void Render(in GameState gameState, float dt) {
            var views = new List<EntityView>();
            
            var coinLines = gameState.PlayState.Region.CoinLines;
            for (var lineIndex = 0; lineIndex < coinLines.Count; lineIndex++) {
                var coinLine = coinLines.Items[lineIndex];
                for (var i = 0; i < coinLine.Count; i++) {
                    var coin = coinLine.Items[i];
                    var entityView = new EntityView();
                    entityView.Id = coin.Id;
                    entityView.Type = EntityType.Coin;
                    entityView.Position = coin.Position.ToUnityVector3();
                    views.Add(entityView);
                }
            }
            
            Sync(views, dt);
        }
    }
}
