using System.Collections.Generic;
using Sources.Core;

namespace Sources {
    
    // TODO: Get rid of this file: either it goes to the file conf or to the scriptable conf.
    public static class CoreConfig {
        public const string GlobalFileName = "global_save.json";
        public const string PlayFileName = "play_saves.json";
        public const float LaneDistance = 2.1f*2;
        public const int GridScale = 10;
        
        public static readonly Dictionary<EntityType, int> EntityScoreValues = new () {
            { EntityType.Coin, 1 },
            { EntityType.Mermaid, 5 },
        };
        public static readonly Dictionary<EntityType, int> EntitySpawnPcts = new () {
            { EntityType.Coin, 80 },
            { EntityType.Mermaid, 40 },
        };
    }
}
