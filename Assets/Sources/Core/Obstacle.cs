using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Sources.Toolbox;

namespace Sources.Core {
    public readonly struct ObstacleInfo {
        public readonly int Z;
        public readonly LaneType LaneType;
        public readonly EntityType EntityType;
        
        public ObstacleInfo(EntityType entityType, LaneType laneType, int z) {
            EntityType = entityType;
            LaneType = laneType;
            Z = z;
        }
    }
    
    public struct Obstacle {
        public Vec3F32 Position;
        public Vec3F32 Size;
        public int Id;
        public EntityType Type;
    }
    
    public struct SegmentInfo {
        public ObstacleInfo[] ObstacleInfos;
    }
    
    public static class ObstacleSystem {
        private static Conf _conf;
        
        public static void Init(in Conf conf) {
            _conf = conf;
        }
        
        [Pure]
        public static List<Obstacle> GenerateObstacles(SegmentInfo segment, int segmentZ) {
            var result = new List<Obstacle>();
            foreach (var obstacleInfo in segment.ObstacleInfos) {
                result.Add(CreateObstacle(obstacleInfo, segmentZ));
            }
            return result;
        }
        
        [Pure]
        private static Obstacle CreateObstacle(ObstacleInfo info, int segmentZ) {
            var result = new Obstacle();
            result.Type = info.EntityType;
            result.Id = EntityManager.NextId;
            
            var x = LaneHelper.GetPosition(info.LaneType);
            if (info.EntityType == EntityType.Trunk) x /= 2;
            
            result.Position = new Vec3F32(x, 0, segmentZ + info.Z);
            result.Size = _conf.Sizes[info.EntityType];
            return result;
        }
    }
}
