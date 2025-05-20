using System;
using System.Collections.Generic;
using Sources.Toolbox;

namespace Sources.States {
    [Serializable]
    public struct Boat {
        public HashSet<int> CollisionIds;
        public SpeedMaxConf SpeedMaxConf;
        public SailConf SailConf;
        public Vec3F32 Size;
        public Vec3F32 Position;  // Reset tous les 1000m pour changement de région
        public float Distance;    // Distance parcourue au total
        public float SailAngle;
        public float SpeedCollisionFactor;
        public int xSign;
        public float SpeedZ;
        public int SpeedZMin;
        public int SpeedX;
        public Health Health;
        public LaneType LaneType;
        public bool SailWindward;
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
    
    [Serializable]
    public struct Health {
        public byte Max;
        public byte Value;
    }
}
