using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    // Services que la plateforme ou le moteur fournit au jeu.
    
    public interface IPlatform {
        string PersistentPath { get; }
        void Log(string message);
        void LogWarn(string message);
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }

    [Serializable]
    public struct PlayerActions {
        public float DeltaSail;
        public float SideMove;
        public bool Cancel;
        public bool Shoot;
        public bool ShowControlsSwitched;
        public bool UsingGamepad;
    }

    [Serializable]
    public struct GameConf {
        public BoatConf BoatConf;
        public CannonConf CannonConf;
        public CoinConf CoinConf;
        public JellyfishConf JellyfishConf;
        public MermaidConf MermaidConf;
        public RegionConf RegionConf;
        public RelicConf RelicConf;
        public WindConf WindConf;
        public bool EnableBoatCollisions; // Pour débugguer uniquement
    }

    public struct RendererConf {
        public Dictionary<EntityType, BoundingBox3F32> BoundingBoxesByEntityType;
        public Dictionary<RegionType, List<Segment>> SegmentsByRegion;
    }
}
