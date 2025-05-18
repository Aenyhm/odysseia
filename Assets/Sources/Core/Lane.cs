using System;
using System.Diagnostics.Contracts;
using Sources.Toolbox;

namespace Sources.Core {
    public enum LaneType : byte {
        Left,
        Center,
        Right
    }
    
    public static class LaneHelper {
        private const float LANE_DISTANCE = 6f;
        
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
        
        public static float GetPosition(LaneType laneType) {
            return GetSign(laneType)*LANE_DISTANCE;
        }
    }
}
