using System.Collections.Generic;
using Sources;
using Sources.Core;

namespace Unity.Scripts.Views.Gameplay {
    public class MermaidManagerView : AbstractEntityManagerView {
        private MermaidManagerView() : base("Mermaid") { }

        protected override ICollection<Entity> GetElements(GameState gameState) {
            return gameState.PlayState.Region.EntitiesByType[EntityType.Mermaid].ToList();
        }
    }
}
