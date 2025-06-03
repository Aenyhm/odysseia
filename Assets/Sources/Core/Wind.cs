using System;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct WindConf {
        public RangeI32 ChangeFreq;
        public float ChangeDuration;
        public int AngleMax;
    }
    
    [Serializable]
    public struct Wind {
        public float CurrentAngle;
        public float TargetAngle;
        public float LastChangeTime;
    }
    
    public static class WindSystem {
        private static Animation _changeAnim;
        
        public static void Init(ref GameState gameState) {
            var windConf = Services.Get<GameConf>().WindConf;

            var wind = new Wind();
            wind.LastChangeTime = Clock.GameTime + windConf.ChangeFreq.Max;
            
            gameState.PlayState.Wind = wind;
        }
        
        public static void Execute(ref GameState gameState, float dt) {
            ref var wind = ref gameState.PlayState.Wind;
            
            if (_changeAnim != null && !_changeAnim.Completed) {
                wind.CurrentAngle = _changeAnim.Update(dt);
            } else {
                var windConf = Services.Get<GameConf>().WindConf;

                var currentTime = Clock.GameTime;

                if (wind.LastChangeTime < currentTime) {
                    wind.TargetAngle = GetNewAngle(wind.CurrentAngle, windConf.AngleMax);
                    wind.LastChangeTime = currentTime + Prng.Range(windConf.ChangeFreq);
                    
                    _changeAnim = new Animation(
                        wind.CurrentAngle, wind.TargetAngle, windConf.ChangeDuration, Easings.InOutSine
                    );
                }
            }
        }
        
        private static int GetNewAngle(float currentAngle, int maxAngle) {
            var result = 0;
            
            if (currentAngle == 0) {
                var sign = Prng.Roll(2) == 0 ? -1 : 1;
                result = maxAngle*sign;
            }
            
            return result;
        }
    }
}
