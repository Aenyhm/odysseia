using Sources.Core;
using UnityEditor;
using UnityEngine;

namespace Unity.Editor {
    [CustomPropertyDrawer(typeof(Entity), true)] // <- true pour inclure les enfants
    public class EntityDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            property.isExpanded = true; // <- Force l'affichage déroulé

            EditorGUI.PropertyField(position, property, label, true);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
