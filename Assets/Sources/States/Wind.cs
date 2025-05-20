using System;

namespace Sources.States {
    
    [Serializable]
    public struct Wind {
        public WindConf Conf;
        public float Angle;
        public float LastChangeDistance;
    }
    
    [Serializable]
    public struct WindConf {
        public int AngleMax;
        public int ChangeDistance;
    }
}
