using System;

namespace Sources.Toolbox {
    public delegate float Easing(float x);
    
    public static class Easings {
        public static float Compute(this Easing easing, float start, float end, float amount) {
            return start + easing(amount)*(end - start);
        }
        
        public static float InOutSine(float x) => (float)(-(Math.Cos(Math.PI*x) - 1)/2);
    }
    
    public class FloatTweening {
        private readonly Easing _easing;
        private readonly float _from;
        private readonly float _to;
        private float _duration;
        private float _elapsed;
        
        private float _timeAmount => _elapsed/_duration;
        
        public bool Completed => _duration == 0 || _timeAmount >= 1;
        
        public FloatTweening(float from, float to, float duration, Easing easing) {
            _from = from;
            _to = to;
            _duration = duration;
            _easing = easing;
        }

        public float Update() {
            _elapsed += Clock.DeltaTime;
            
            if (Completed) {
                Reset();
                return _to;
            }
            
            return _easing.Compute(_from, _to, _timeAmount);
        }
        
        private void Reset() {
            _duration = 0;
        }
    }
}
