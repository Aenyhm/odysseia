using Sources;
using UnityEngine;

namespace Unity {
    public class OdysseiaUnity : MonoBehaviour {
        private Game _game;
        
        private void Start() {
            _game = new Game(new UnityPlatform());
        }
        
        private void FixedUpdate() => _game.FixedUpdate(Time.fixedDeltaTime);
        private void Update() => _game.Update(Time.deltaTime);
    }
}
