using System;
using System.Collections.Generic;
using Sources.Core;
using Sources.Toolbox;

namespace Sources {
    // Services that the platform/engine provides to the game.
    
    public interface IPlatform {
        string PersistentPath { get; }
        void Log(string message);
        void LogWarn(string message);
        string Serialize(object obj);
        T Deserialize<T>(string json);
    }

    [Serializable]
    public struct GameInput {
        public float MouseDeltaX;
        public float HorizontalAxis;
        public bool MouseButtonLeftDown;
        public bool Escape;
        public bool Space;
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
        public bool EnableBoatCollisions;
    }

    public struct RendererConf {
        public Dictionary<EntityType, Vec3F32> Sizes;
        public Dictionary<RegionType, List<Segment>> SegmentsByRegion;
    }
}
