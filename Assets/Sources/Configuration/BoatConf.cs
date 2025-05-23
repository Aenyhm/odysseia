using System;

namespace Sources.Configuration {
    
    [Serializable]
    public struct BoatConf {
        public SpeedMaxConf SpeedMaxConf;
        public SailConf SailConf;
        public float PositionY;
        public float SpeedCollisionFactor;
        public float SpeedZStart;
        public float SpeedZMin;
        public float SpeedX;
        public byte HealthMax;
    }
    
    [Serializable]
    public struct SpeedMaxConf {
        public float Multiplier;
        public int DistanceStep;
        public int Min;
        public int Max;
    }
    
    [Serializable]
    public struct SailConf {
        public float AngleMax;
        public float WindwardAngleRange;
    }
}
