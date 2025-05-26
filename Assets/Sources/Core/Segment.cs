using System;
using System.Collections.Generic;

namespace Sources.Core {
    public enum SegmentLength : byte { L100 = 100, L50 = 50 }

    [Serializable]
    public struct Segment {
        public List<EntityCell> EntityCells;
        public SegmentLength Length;
        public RegionType RegionType;
    }

    [Serializable]
    public struct EntityCell {
        public EntityType Type;
        public int X;
        public int Y;
    }
}
