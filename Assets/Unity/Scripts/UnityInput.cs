using System;
using System.Collections.Generic;
using Sources;
using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public enum PlayerActionType {
        Validate,
        Cancel,
        Tab,
        Shoot,
        Help,
    }
    
    public readonly struct KeysByInput {
        public readonly KeyCode KeyboardKey;
        public readonly KeyCode GamepadKey;
        
        public KeysByInput(KeyCode keyboardKey, KeyCode gamepadKey) {
            KeyboardKey = keyboardKey;
            GamepadKey = gamepadKey;
        }
    }
    
    public class UnityInput {
        private static UnityInput _instance;
        public PlayerActions Actions;
        
        public static UnityInput Instance => _instance ??= new UnityInput();

        private UnityInput() {
            foreach (var joystickName in Input.GetJoystickNames()) {
                if (joystickName != "") {
                    Actions.UsingGamepad = true;
                    break;
                }
            }
        }
        
        // Je préfère contrôler le mapping ici plutôt que dans les settings, car on peut imaginer
        // que le joueur pourra les changer comme il le souhaite dans une future version.
        // Xbox Controller mapping: https://i.sstatic.net/9czAW.jpg
        private readonly Dictionary<PlayerActionType, KeysByInput> _keysByInputsByActionType = new() {
            { PlayerActionType.Validate, new KeysByInput(KeyCode.Return, KeyCode.Joystick1Button0) },
            { PlayerActionType.Cancel,   new KeysByInput(KeyCode.Escape, KeyCode.Joystick1Button6) },
            { PlayerActionType.Tab,      new KeysByInput(KeyCode.Tab,    KeyCode.Joystick1Button4) },
            { PlayerActionType.Shoot,    new KeysByInput(KeyCode.Space,  KeyCode.Joystick1Button0) },
            { PlayerActionType.Help,     new KeysByInput(KeyCode.H,      KeyCode.Joystick1Button3) },
        };

        public bool GetAction(PlayerActionType actionType) {
            var _keysByInput = _keysByInputsByActionType[actionType];
            
            return IsActionDone(_keysByInput);
        }

        public void Clear() {
            var usingGamepad = Actions.UsingGamepad;
            Actions = default;
            Actions.UsingGamepad = usingGamepad;
        }
        
        public void Update() {
            Actions.SideMove = GetMaxFloatValue(Actions.SideMove, Input.GetAxisRaw("Horizontal"));
            Actions.DeltaSail = GetMaxFloatValue(Actions.DeltaSail, GetSailValue());
            Actions.Cancel |= GetAction(PlayerActionType.Cancel);
            Actions.Shoot |= GetAction(PlayerActionType.Shoot);
            Actions.ShowControlsSwitched |= GetAction(PlayerActionType.Help);
        }

        private bool IsActionDone(KeysByInput keysByInput) {
            var result = false;

            if (Input.GetKeyDown(keysByInput.KeyboardKey)) {
                result = true;
                Actions.UsingGamepad = false;
            } else if (Input.GetKeyDown(keysByInput.GamepadKey)) {
                result = true;
                Actions.UsingGamepad = true;
            }
            
            return result;
        }

        private float GetSailValue() {
            float result;
            
            var mouse = Input.GetAxisRaw("Mouse X");
            if (mouse != 0 && Input.GetMouseButton(0)) {
                Actions.UsingGamepad = false;
                result = mouse;
            } else {
                result = Input.GetAxisRaw("RHorizontal");
                if (!Maths.FloatEquals(result, 0)) {
                    Actions.UsingGamepad = true;
                }
            }
            
            return result;
        }

        private static float GetMaxFloatValue(float previous, float value) {
            return Math.Abs(value) > Math.Abs(previous) ? value : previous;
        }
    }
}
