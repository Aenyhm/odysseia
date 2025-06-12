using System;

namespace Sources.Toolbox {
    // J'aurais pu regrouper ces 2 structures en 1 en utilisant
    // la généricité, mais ils n'auraient plus été sérialisables.
    
    [Serializable]
    public struct RangeI32 {
        public int Min;
        public int Max;
        
        public RangeI32(int min, int max) {
            Min = min;
            Max = max;
        }
    }
    
    [Serializable]
    public struct RangeF32 {
        public float Min;
        public float Max;
        
        public RangeF32(float min, float max) {
            Min = min;
            Max = max;
        }
    }

    public static class Maths {
        
        // Pour comparer 2 nombres flottants, à cause de leur précision, on regarde s'ils sont presque équivalents.
        public static bool FloatEquals(float a, float b) {
            return Math.Abs(a - b) < float.Epsilon;
        }
        
        public static float MoveTowards(float current, float target, float maxDelta) {
            return Math.Abs(target - current) <= maxDelta ? target : current + Math.Sign(target - current)*maxDelta;
        }
    }
}
