using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class RockManagerView : AbstractEntityManagerView {
        protected override EntityType Type => EntityType.Rock;

        protected override void Init(GameObject go) {
            go.transform.GetChild(0).RotateOnAxis(Axis.Z, Random.Range(0, 360));
        }

        public override void Render(in GameState gameState, float dt) {
            var entitiesById = new Dictionary<int, Entity>();
            foreach (var e in gameState.PlayState.Region.Entities) {
                if (e.Type == EntityType.Rock) {
                    entitiesById[e.Id] = e;
                }
            }
            
            Sync(entitiesById.Keys);
            
            foreach (var (id, go) in _gosById) {
                var e = entitiesById[id];
                var pos = EntityLogic.GetPosition(e.Type, e.Coords);
                
                go.transform.localPosition = pos.ToUnityVector3();
            }
        }
    }
}
