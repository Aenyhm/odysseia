using System;
using System.Diagnostics.Contracts;

namespace Sources.Toolbox {
    
    [Serializable]
    public struct BoundingBox3F32 {
        public Vec3F32 Min;
        public Vec3F32 Max;
    } 
    
    public static class Collisions {

        [Pure]
        public static bool CheckCollisionBoxes(Vec3F32 pos1, BoundingBox3F32 box1, Vec3F32 pos2, BoundingBox3F32 box2) {
            var min1 = pos1 + box1.Min;
            var max1 = pos1 + box1.Max;
            var min2 = pos2 + box2.Min;
            var max2 = pos2 + box2.Max;

            return (min1.X <= max2.X && max1.X >= min2.X) &&
                   (min1.Y <= max2.Y && max1.Y >= min2.Y) &&
                   (min1.Z <= max2.Z && max1.Z >= min2.Z);
        }
    }
}
