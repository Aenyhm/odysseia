using System;

namespace Sources.Toolbox {
    public static class Collisions {
        public static bool CheckAabb(Vec3F32 p1, Vec3F32 s1, Vec3F32 p2, Vec3F32 s2) {
            return Math.Abs(p1.X - p2.X) <= (s1.X + s2.X)*0.5f
                && Math.Abs(p1.Y - p2.Y) <= (s1.Y + s2.Y)*0.5f
                && Math.Abs(p1.Z - p2.Z) <= (s1.Z + s2.Z)*0.5f;
        }
    }
}
