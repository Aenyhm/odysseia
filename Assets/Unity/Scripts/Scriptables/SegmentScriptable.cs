using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Scriptables {
    [CreateAssetMenu(fileName = "Segment", menuName = "Scriptable Objects/Segment")]
    public class SegmentScriptable : ScriptableObject {
        public Segment Segment;
    }
}
