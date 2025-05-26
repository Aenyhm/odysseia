using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CoinManagerView : AbstractEntityManagerView {
        [SerializeField] private float _rotateSpeed = 150f;
        
        protected override EntityType Type => EntityType.Coin;
        
        protected override void Init(GameObject go) {
            go.transform.RotateOnAxis(Axis.Y, 0);
        }

        public override void Render(in GameState gameState, float dt) {
            var coins = gameState.PlayState.Region.Coins;
            
            var entitiesById = new Dictionary<int, Coin>(coins.Count);
            for (var i = 0; i < coins.Count; i++) {
                var coin = coins.Items[i];
                entitiesById.Add(coin.Id, coin);
            }
            
            Sync(entitiesById.Keys);
            
            foreach (var (id, go) in _gosById) {
                var coin = entitiesById[id];
                go.transform.localPosition = coin.Position.ToUnityVector3();
                go.transform.RotateOnAxis(Axis.Y, go.transform.localEulerAngles.y + dt*_rotateSpeed);
            }
        }
    }
}
