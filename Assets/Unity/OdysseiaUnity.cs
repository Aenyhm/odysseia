using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity {
    public class OdysseiaUnity : MonoBehaviour, IPlatform {
        private readonly UnityRenderer _renderer = new();
        private IGame _game;
        
        public void Log(string message) {
            Debug.Log(message);
        }

        public void AddEntityView(Entity entity) {
            _renderer.Link(entity);
        }
        public void RemoveEntityView(Entity entity) {
            _renderer.Unlink(entity);
        }

        private void Start() {
            _game = new Game(this);
        }

        private void Update() {
            _game.Update(Time.deltaTime, ReadInput());
            _renderer.Update();
        }
        
        private static GameInput ReadInput() {
            GameInput input;
            input.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            return input;
        }
    }
}
