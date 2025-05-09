using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts {
    public class OdysseiaUnity : MonoBehaviour, IPlatform {
        private readonly UnityRenderer _renderer = new();
        private Game _game;
        
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

        private void FixedUpdate() {
            _game.Update(Time.fixedDeltaTime, ReadInput());
        }
        
        private void Update() {
            _renderer.Update();
        }
        
        private static GameInput ReadInput() {
            GameInput input;
            input.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            input.MouseDeltaX = Input.mousePositionDelta.x;
            input.MouseButtonLeftDown = Input.GetMouseButton(0);
            return input;
        }
    }
}
