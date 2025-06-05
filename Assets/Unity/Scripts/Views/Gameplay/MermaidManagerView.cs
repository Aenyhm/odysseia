using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class MermaidManagerView : AbstractManagerView<Entity> {
        private MermaidManagerView() : base("Mermaid") { }
        
        public override void Render(GameState gameState, float dt) {
            var entitiesById = new Dictionary<int, Entity>();
            foreach (var e in gameState.PlayState.Region.Entities) {
                if (e.Type == EntityType.Mermaid) {
                    entitiesById[e.Id] = e;
                }
            }
            
            Sync(entitiesById);
        }

        protected override void InitChild(GameObject go, Entity data) {
            go.transform.localPosition = data.Position.ToUnityVector3();
        }
    }
}
