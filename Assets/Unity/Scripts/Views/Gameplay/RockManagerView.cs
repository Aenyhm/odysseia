using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class RockManagerView : AbstractEntityManagerView {
        private RockManagerView() : base("Rock") { }
        
        protected override ICollection<Entity> GetElements(GameState gameState) {
            return gameState.PlayState.Region.EntitiesByType[EntityType.Rock].ToList();
        }

        protected override void InitChild(GameObject go, Entity data) {
            base.InitChild(go, data);
            go.transform.GetChild(0).RotateOnAxis(Axis.Z, Random.Range(0, 360));
        }
    }
}
