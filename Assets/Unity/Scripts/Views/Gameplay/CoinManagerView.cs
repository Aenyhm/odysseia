using System.Collections.Generic;
using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Views.Gameplay {
    public class CoinManagerView : AbstractManagerView<Coin> {
        [SerializeField] private float _rotateSpeed = 150f;
        
        private CoinManagerView() : base("Coin") { }

        public override void Render(in GameState gameState, float dt) {
            var coins = gameState.PlayState.Region.Coins;
            
            var entitiesById = new Dictionary<int, Coin>(coins.Count);
            for (var i = 0; i < coins.Count; i++) {
                var coin = coins.Items[i];
                entitiesById.Add(coin.Id, coin);
            }
            
            Sync(entitiesById);
            
            foreach (var go in _gosById.Values) {
                go.transform.RotateOnAxis(Axis.Y, go.transform.localEulerAngles.y + dt*_rotateSpeed);
            }
        }

        protected override void InitChild(GameObject go, Coin data) {
            go.transform.localPosition = data.Position.ToUnityVector3();
            go.transform.RotateOnAxis(Axis.Y, 0);
        }
    }
}
