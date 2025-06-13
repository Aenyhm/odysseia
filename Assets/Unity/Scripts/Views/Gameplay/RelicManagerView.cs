using System.Collections.Generic;
using Sources;
using Sources.Core;

namespace Unity.Scripts.Views.Gameplay {
    public class RelicManagerView : AbstractEntityManagerView {
        private RelicManagerView() : base("Relic") { }

        protected override ICollection<Entity> GetElements(GameState gameState) {
            return gameState.PlayState.Region.EntitiesByType[EntityType.Relic].ToList();
        }
    }
}
