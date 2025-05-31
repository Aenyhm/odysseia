using System.Collections.Generic;
using Sources;
using UnityEngine;

namespace Unity.Scripts {
    public static class UnityInput {
        private static readonly Dictionary<KeyCode, bool> _statesByKey = new();
        
        public static GameInput Read() {
            GameInput input;
            input.HorizontalAxis = Input.GetAxisRaw("Horizontal");
            input.MouseDeltaX = Input.mousePositionDelta.x;
            input.MouseButtonLeftDown = Input.GetMouseButton(0);
            input.Escape = IsKeyPressed(KeyCode.Escape);
            input.Space = IsKeyPressed(KeyCode.Space);
            return input;
        }
        
        /// <summary>
        /// Permet de mieux gérer l'état d'une touche.
        /// </summary>
        /// <param name="code">code de la touche</param>
        /// <returns>vrai au changement d'état si la touche est enfoncée.</returns>
        private static bool IsKeyPressed(KeyCode code) {
            var result = false;
            
            _statesByKey.TryAdd(code, false);
            
            var newState = Input.GetKeyDown(code);
            
            if (_statesByKey[code] != newState) {
                _statesByKey[code] = newState;
                result = newState;
            }
            
            return result;
        }
    }
}
