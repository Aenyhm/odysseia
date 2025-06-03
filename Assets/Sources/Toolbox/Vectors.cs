using System;

namespace Sources.Toolbox {
    
    [Serializable]
    public struct Vec2I32 : IEquatable<Vec2I32> {
        public int X;
        public int Y;
        public Vec2I32(int x = 0, int y = 0) { X = x; Y = y; }
        public override string ToString() => $"Vec2I32({X}, {Y})";
        public static Vec2I32 One => new(1, 1);

        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override bool Equals(object obj) => base.Equals(obj);
        public bool Equals(Vec2I32 other) =>  X == other.X && Y == other.Y;

        public static bool operator ==(Vec2I32 a, Vec2I32 b) => a.Equals(b);
        public static bool operator !=(Vec2I32 a, Vec2I32 b) => !a.Equals(b);

        public static Vec2I32 operator +(Vec2I32 a, Vec2I32 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vec2I32 operator -(Vec2I32 a, Vec2I32 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vec2I32 operator *(Vec2I32 a, Vec2I32 b) => new(a.X*b.X, a.Y*b.Y);
        public static Vec2I32 operator *(Vec2I32 v, int factor) => new(v.X*factor, v.Y*factor);
        public static Vec2I32 operator /(Vec2I32 a, Vec2I32 b) => new(a.X/b.X, a.Y/b.Y);
        public static Vec2I32 operator /(Vec2I32 v, int factor) => new(v.X/factor, v.Y/factor);
        public static implicit operator Vec2F32(Vec2I32 v) => new(v.X, v.Y);
    }
    
    [Serializable]
    public struct Vec2F32 {
        public float X;
        public float Y;
        public Vec2F32(float x = 0f, float y = 0f) { X = x; Y = y; }
        public override string ToString() => $"Vec2F32({X}, {Y})";
        public static Vec2F32 One => new(1, 1);
        
        public static Vec2F32 operator +(Vec2F32 a, Vec2F32 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vec2F32 operator -(Vec2F32 a, Vec2F32 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vec2F32 operator *(Vec2F32 a, Vec2F32 b) => new(a.X*b.X, a.Y*b.Y);
        public static Vec2F32 operator *(Vec2F32 v, float factor) => new(v.X*factor, v.Y*factor);
        public static Vec2F32 operator /(Vec2F32 a, Vec2F32 b) => new(a.X/b.X, a.Y/b.Y);
        public static Vec2F32 operator /(Vec2F32 v, float factor) => new(v.X/factor, v.Y/factor);
    }
    
    [Serializable]
    public struct Vec3F32 : IEquatable<Vec3F32> {
        public float X;
        public float Y;
        public float Z;
        public Vec3F32(float x = 0f, float y = 0f, float z = 0f) { X = x; Y = y; Z = z; }
        public override string ToString() => $"Vec3F32({X}, {Y}, {Z})";
        public static Vec3F32 One => new(1, 1, 1);
        
        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
        public override bool Equals(object obj) => base.Equals(obj);
        public bool Equals(Vec3F32 other) => (
            Maths.FloatEquals(X, other.X) &&
            Maths.FloatEquals(Y, other.Y) &&
            Maths.FloatEquals(Z, other.Z)
        );

        public static bool operator ==(Vec3F32 a, Vec3F32 b) => a.Equals(b);
        public static bool operator !=(Vec3F32 a, Vec3F32 b) => !a.Equals(b);

        public static Vec3F32 operator +(Vec3F32 a, Vec3F32 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3F32 operator -(Vec3F32 a, Vec3F32 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3F32 operator *(Vec3F32 a, Vec3F32 b) => new(a.X*b.X, a.Y*b.Y, a.Z*b.Z);
        public static Vec3F32 operator *(Vec3F32 v, float factor) => new(v.X*factor, v.Y*factor, v.Z*factor);
        public static Vec3F32 operator /(Vec3F32 a, Vec3F32 b) => new(a.X/b.X, a.Y/b.Y, a.Z/b.Z);
        public static Vec3F32 operator /(Vec3F32 v, float factor) => new(v.X/factor, v.Y/factor, v.Z/factor);
    }
}
