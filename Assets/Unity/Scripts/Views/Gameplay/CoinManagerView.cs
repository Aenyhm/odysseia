using System.Collections.Generic;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CoinManagerView : AbstractEntityManagerView {
        [SerializeField] private float _rotateSpeed = 150f;
        
        private CoinManagerView() : base("Coin") { }

        protected override ICollection<Entity> GetElements(GameState gameState) {
            return gameState.PlayState.Region.EntitiesByType[EntityType.Coin].ToList();
        }

        protected override void InitChild(GameObject go, Entity data) {
            base.InitChild(go, data);
            go.transform.RotateOnAxis(Axis.Y, 0);
        }

        protected override void UpdateChild(GameObject go, Entity data) {
            base.UpdateChild(go, data);
            go.transform.RotateOnAxis(Axis.Y, go.transform.localEulerAngles.y + _rotateSpeed*Clock.DeltaTime);
        }
    }
}
