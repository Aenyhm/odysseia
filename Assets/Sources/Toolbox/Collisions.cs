using System;
using System.Diagnostics.Contracts;

namespace Sources.Toolbox {
    public static class Collisions {
        [Pure]
        public static bool CheckLines(float pos1, float size1, float pos2, float size2) {
            return Math.Abs(pos1 - pos2) <= (size1 + size2)*0.5f;
        }
        
        [Pure]
        public static bool CheckAabb(Vec3F32 p1, Vec3F32 s1, Vec3F32 p2, Vec3F32 s2) {
            return CheckLines(p1.X, s1.X, p2.X, s2.X) &&
                   CheckLines(p1.Y, s1.Y, p2.Y, s2.Y) &&
                   CheckLines(p1.Z, s1.Z, p2.Z, s2.Z);
        }
    }
}
