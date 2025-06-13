using System.Collections.Generic;
using Sources;
using Sources.Core;

namespace Unity.Scripts.Views.Gameplay {
    public class JellyfishManagerView : AbstractEntityManagerView {
        private JellyfishManagerView() : base("Jellyfish") { }
        
        protected override ICollection<Entity> GetElements(GameState gameState) {
            return gameState.PlayState.Region.EntitiesByType[EntityType.Jellyfish].ToList();
        }
    }
}
