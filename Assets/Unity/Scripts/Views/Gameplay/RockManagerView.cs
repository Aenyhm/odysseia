using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class RockManagerView : AbstractManagerView<Entity> {
        private RockManagerView() : base("Rock") { }

        public override void Render(in GameState gameState, float dt) {
            var entitiesById = new Dictionary<int, Entity>();
            foreach (var e in gameState.PlayState.Region.Entities) {
                if (e.Type == EntityType.Rock) {
                    entitiesById[e.Id] = e;
                }
            }
            
            Sync(entitiesById);
        }

        protected override void InitChild(GameObject go, Entity data) {
            var pos = EntityLogic.GetPosition(data.Type, data.Coords);
            go.transform.localPosition = pos.ToUnityVector3();
            
            go.transform.GetChild(0).RotateOnAxis(Axis.Z, Random.Range(0, 360));
        }
    }
}
