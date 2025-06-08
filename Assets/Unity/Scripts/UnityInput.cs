using System;
using Sources;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public enum PlayerActionType {
        Validate,
        Cancel,
        Tab,
    }
    
    // Xbox Controller mapping: https://i.sstatic.net/9czAW.jpg
    public class UnityInput {
        private PlayerActions _actions;

        public PlayerActions Actions => _actions;
        
        public UnityInput() {
            foreach (var joystickName in Input.GetJoystickNames()) {
                if (joystickName != "") {
                    _actions.UsingGamepad = true;
                    break;
                }
            }
        }
        
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

        public void Clear() {
            var usingGamepad = _actions.UsingGamepad;
            _actions = default;
            _actions.UsingGamepad = usingGamepad;
        }
        
        public void Update() {
            _actions.SideMove = GetMaxFloatValue(_actions.SideMove, Input.GetAxisRaw("Horizontal"));
            _actions.DeltaSail = GetMaxFloatValue(_actions.DeltaSail, GetSailValue());
            
            // Je préfère contrôler le mapping ici plutôt que dans les settings, car on peut imaginer
            // que le joueur pourra les changer comme il le souhaite dans une future version.
            _actions.Cancel |= IsActionDone(KeyCode.Escape, KeyCode.Joystick1Button6);
            _actions.Shoot |= IsActionDone(KeyCode.Space, KeyCode.Joystick1Button0);
        }

        private bool IsActionDone(KeyCode keyboardKey, KeyCode gamepadKey) {
            var result = false;

            if (Input.GetKeyDown(keyboardKey)) {
                result = true;
                _actions.UsingGamepad = false;
            } else if (Input.GetKeyDown(gamepadKey)) {
                result = true;
                _actions.UsingGamepad = true;
            }
            
            return result;
        }

        private float GetSailValue() {
            float result;
            
            var mouse = Input.GetAxisRaw("Mouse X");
            if (mouse != 0 && Input.GetMouseButton(0)) {
                _actions.UsingGamepad = false;
                result = mouse;
            } else {
                result = Input.GetAxisRaw("RHorizontal");
                if (!Maths.FloatEquals(result, 0)) {
                    _actions.UsingGamepad = true;
                }
            }
            
            return result;
        }

        private static float GetMaxFloatValue(float previous, float value) {
            return Math.Abs(value) > Math.Abs(previous) ? value : previous;
        }
    }
}
