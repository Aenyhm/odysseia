using Sources.Toolbox;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor {
    [CustomPropertyDrawer(typeof(Vec3F32))]
    public class Vec3F32Drawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var x = property.FindPropertyRelative("x");
            var y = property.FindPropertyRelative("y");
            var z = property.FindPropertyRelative("z");

            Vector3 vec = new(x.floatValue, y.floatValue, z.floatValue);
            vec = EditorGUI.Vector3Field(position, label, vec);
            x.floatValue = vec.x;
            y.floatValue = vec.y;
            z.floatValue = vec.z;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
