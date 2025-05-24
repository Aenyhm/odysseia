using System;
using System.Diagnostics.Contracts;
using Sources.Core;
using Sources.Toolbox;

namespace Sources.Mechanics {
    public static class LaneMechanics {
        [Pure]
        public static LaneType GetDelta(LaneType laneType, int delta) {
            var index = (int)laneType + delta;
            
            return (LaneType)Math.Clamp(index, 0, Enums.Count<LaneType>() - 1);
        }
        
        [Pure]
        public static int GetSign(LaneType laneType) {
            return laneType switch {
                LaneType.Left => -1,
                LaneType.Center => 0,
                LaneType.Right => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(laneType), laneType, null)
            };
        }
        
        [Pure]
        public static float GetPosition(LaneType laneType, float laneDistance) {
            return GetSign(laneType)*laneDistance;
        }
    }
}
