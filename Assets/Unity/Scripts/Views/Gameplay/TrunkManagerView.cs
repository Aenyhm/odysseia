using System.Collections.Generic;
using Sources;
using Sources.Core;

namespace Unity.Scripts.Views.Gameplay {
    public class TrunkManagerView : AbstractEntityManagerView {
        private TrunkManagerView() : base("Trunk") { }

        protected override ICollection<Entity> GetElements(GameState gameState) {
            return gameState.PlayState.Region.EntitiesByType[EntityType.Trunk].ToList();
        }
    }
}
