using Sources;
using UnityEngine;

namespace Unity {
    public class UnityPlatform : Platform {
        public UnityPlatform() : base(new UnityRenderer()) { }

        public override void Log(string message) {
            Debug.Log(message);
        }
        
        public override float GetHorizontalAxisInput() {
             return Input.GetAxisRaw("Horizontal");
        }
    }
}
