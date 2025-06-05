using System;
using Sources;
using UnityEngine;

namespace Unity.Scripts {
    public static class UnityInput {
        private static GameInput _input;

        public static GameInput Data => _input;
        
        public static void Clear() {
            _input = default;
        }

        public static void Update() {
            _input.HorizontalAxis = GetMaxFloatValue(_input.HorizontalAxis, Input.GetAxisRaw("Horizontal"));
            _input.MouseDeltaX = GetMaxFloatValue(_input.MouseDeltaX, Input.mousePositionDelta.x);
            _input.MouseButtonLeftDown |= Input.GetMouseButton(0);
            _input.Escape |= Input.GetKeyDown(KeyCode.Escape);
            _input.Space |= Input.GetKeyDown(KeyCode.Space);
        }

        private static float GetMaxFloatValue(float previous, float value) {
            return Math.Abs(value) > Math.Abs(previous) ? value : previous;
        }
    }
}
