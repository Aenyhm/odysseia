using Sources.Toolbox;
using UnityEngine;

namespace Unity.Scripts {
    public static class UnityExtensions {
        public static Vector3 ToUnityVector3(this Vec3F32 v) => new(v.x, v.y, v.z);
    }
}
