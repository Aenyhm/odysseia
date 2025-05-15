using System;

namespace Sources.Toolbox {
    [Serializable]
    public struct Vec3F32 {
        public float X;
        public float Y;
        public float Z;
        
        public Vec3F32(float x = 0, float y = 0, float z = 0) {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() {
            return $"Vec3F32({X}, {Y}, {Z})";
        }

        public static Vec3F32 operator +(Vec3F32 a, Vec3F32 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3F32 operator -(Vec3F32 a, Vec3F32 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3F32 operator *(Vec3F32 a, Vec3F32 b) => new(a.X*b.X, a.Y*b.Y, a.Z*b.Z);
        public static Vec3F32 operator *(Vec3F32 v, float factor) => new(v.X*factor, v.Y*factor, v.Z*factor);
        public static Vec3F32 operator /(Vec3F32 a, Vec3F32 b) => new(a.X/b.X, a.Y/b.Y, a.Z/b.Z);
        public static Vec3F32 operator /(Vec3F32 v, float factor) => new(v.X/factor, v.Y/factor, v.Z/factor);
    }
}
