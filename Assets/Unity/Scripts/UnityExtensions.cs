using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public enum Axis : byte { X, Y, Z }
    
    public static class UnityExtensions {
        public static Vector3 ToUnityVector3(this Vec3F32 v) => new(v.X, v.Y, v.Z);
        public static Color ToUnityColor(this System.Drawing.Color c) => new Color(c.R, c.G, c.B, c.A)/255f;

        public static void MoveOnAxis(this Transform transform, Axis axis, float value) {
            var newPosition = transform.localPosition;
            
            switch (axis) {
                case Axis.X: newPosition.x = value; break;
                case Axis.Y: newPosition.y = value; break;
                case Axis.Z: newPosition.z = value; break;
            }

            transform.localPosition = newPosition;
        }
                
        public static void RotateOnAxis(this Transform transform, Axis axis, float value) {
            var x = axis == Axis.X ? value : transform.localEulerAngles.x;
            var y = axis == Axis.Y ? value : transform.localEulerAngles.y;
            var z = axis == Axis.Z ? value : transform.localEulerAngles.z;

            transform.localRotation = Quaternion.Euler(x, y, z);
        }
        
        public static void ScaleOnAxis(this Transform transform, Axis axis, float value) {
            var newScale = transform.localScale;
            
            switch (axis) {
                case Axis.X: newScale.x = value; break;
                case Axis.Y: newScale.y = value; break;
                case Axis.Z: newScale.z = value; break;
            }

            transform.localScale = newScale;
        }
    }
}
