using System;

namespace Sources.Toolbox {
    [Serializable]
    public struct Vec3F32 {
        public float x;
        public float y;
        public float z;
        
        public static Vec3F32 one = new(1, 1, 1);
        
        public Vec3F32(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public static Vec3F32 operator +(Vec3F32 a, Vec3F32 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vec3F32 operator -(Vec3F32 a, Vec3F32 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vec3F32 operator *(Vec3F32 a, Vec3F32 b) => new(a.x*b.x, a.y*b.y, a.z*b.z);
        public static Vec3F32 operator *(Vec3F32 v, float factor) => new(v.x*factor, v.y*factor, v.z*factor);
        public static Vec3F32 operator /(Vec3F32 a, Vec3F32 b) => new(a.x/b.x, a.y/b.y, a.z/b.z);
        public static Vec3F32 operator /(Vec3F32 v, float factor) => new(v.x/factor, v.y/factor, v.z/factor);
    }
}
