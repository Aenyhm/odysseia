using System;
using Sources;
using UnityEngine;

namespace Unity.Scripts {
    public enum PlayerActionType {
        Validate,
        Cancel,
        Tab,
    }
    
    // Xbox Controller mapping: https://i.sstatic.net/9czAW.jpg
    public static class UnityInput {
        private static PlayerActions _actions;

        public static PlayerActions Actions => _actions;
        
        private static bool _validate => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0);
        private static bool _cancel => Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button6);
        private static bool _tab => Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Joystick1Button4);

        public static bool GetAction(PlayerActionType actionType) {
            return actionType switch {
                PlayerActionType.Validate => _validate,
                PlayerActionType.Cancel => _cancel,
                PlayerActionType.Tab => _tab,
                _ => false
            };
        }

        public static void Clear() {
            _actions = default;
        }
        
        public static void Update() {
            _actions.SideMove = GetMaxFloatValue(_actions.SideMove, Input.GetAxisRaw("Horizontal"));
            _actions.DeltaSail = GetMaxFloatValue(_actions.DeltaSail, GetSailValue());
            
            // Je préfère contrôler le mapping ici plutôt que dans les settings, car on peut imaginer
            // que le joueur pourra les changer comme il le souhaite dans une future version.
            _actions.Cancel |= _cancel;
            _actions.Shoot |= Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0);
        }
        
        private static float GetSailValue() {
            var mouse = Input.GetAxisRaw("Mouse X");
            if (mouse != 0 && Input.GetMouseButton(0)) return mouse;
            
            return Input.GetAxisRaw("RHorizontal");
        }

        private static float GetMaxFloatValue(float previous, float value) {
            return Math.Abs(value) > Math.Abs(previous) ? value : previous;
        }
    }
}
