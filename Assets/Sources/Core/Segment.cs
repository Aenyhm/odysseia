using System;
using System.Collections.Generic;
using Sources.Toolbox;

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
        public int X;
        public int Y;
        public EntityType Type;
    }
    
    public static class SegmentLogic {
        // On génère des tronçons de 100m d'obstacles et 50m d'ennemis
        // en laissant une zone tranquille entre les portails.
        public static Segment[] GenerateSegments(List<Segment> availableSegments, int filledSegmentsDistance) {
            var result = Array.Empty<Segment>();
            
            if (availableSegments.Count != 0) {
                var segments = new List<Segment>();

                var generatedDistance = 0;
                
                while (generatedDistance < filledSegmentsDistance - (int)SegmentLength.L50) {
                    var segmentIndex = Prng.Roll(availableSegments.Count);
                    var segment = availableSegments[segmentIndex];
         
                    segments.Add(segment);
                    generatedDistance += (int)segment.Length;
                }
                
                if (generatedDistance != filledSegmentsDistance) {
                    throw new Exception(
                        $"Wrong segment distance generated: {generatedDistance}; must be {filledSegmentsDistance}."
                    );
                }
                
                result = segments.ToArray();
                
                Prng.Shuffle(result);
            }
            
            return result;
        }
    }
}
