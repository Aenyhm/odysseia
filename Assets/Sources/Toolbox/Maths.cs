using System;

namespace Sources.Toolbox {
    [Serializable]
    public struct RangeI32 {
        public int Min;
        public int Max;
    }
    
    public static class Maths {
        public static bool FloatEquals(float a, float b) {
            return Math.Abs(a - b) < float.Epsilon;
        }
        
        public static float MoveTowards(float current, float target, float maxDelta) {
            return Math.Abs(target - current) <= maxDelta ? target : current + Math.Sign(target - current)*maxDelta;
        }
    }
}
