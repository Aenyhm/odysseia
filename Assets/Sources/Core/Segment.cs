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
        // Note: J'aurais pu utiliser Vec2I32, mais je ne l'avais pas encore créé et comme il y a déjà
        // des données enregistrées dans ce format, je ne voudrais pas avoir à refaire tous les tronçons.
        public int X;
        public int Y;
        public EntityType Type;
    }
    
    public static class SegmentLogic {
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
                
                #if UNITY_EDITOR
                if (generatedDistance != filledSegmentsDistance) {
                    throw new Exception(
                        $"Wrong segment distance generated: {generatedDistance}; must be {filledSegmentsDistance}."
                    );
                }
                #endif
                
                result = segments.ToArray();
                
                Prng.Shuffle(result);
            }
            
            return result;
        }
    }
}
