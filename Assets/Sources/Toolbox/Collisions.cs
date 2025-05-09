using System;
using Sources.Core;

namespace Sources.Toolbox {
    public static class Collisions {
        public static bool CheckAabb(Transform t1, Transform t2) {
            return Math.Abs(t1.position.x - t2.position.x) <= (t1.size.x + t2.size.x)*0.5f
                && Math.Abs(t1.position.y - t2.position.y) <= (t1.size.y + t2.size.y)*0.5f
                && Math.Abs(t1.position.z - t2.position.z) <= (t1.size.z + t2.size.z)*0.5f;
        }
    }
}
