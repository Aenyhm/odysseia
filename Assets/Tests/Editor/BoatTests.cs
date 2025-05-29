using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sources.Core;

namespace Tests.Editor {
    
    [TestFixture]
    public class BoatTests {
        
        public static IEnumerable<TestCaseData> Cases {
            get {
                var conf = new SpeedMaxConf { DistanceStep = 500, Multiplier = 1.1f, Min = 20, Max = 40 };

                yield return new TestCaseData(conf,     0).Returns(conf.Min);
                yield return new TestCaseData(conf,   499).Returns(conf.Min);
                yield return new TestCaseData(conf,   500).Returns((float)(conf.Min*Math.Pow(conf.Multiplier, 1)));
                yield return new TestCaseData(conf,   750).Returns((float)(conf.Min*Math.Pow(conf.Multiplier, 1)));
                yield return new TestCaseData(conf,  1200).Returns((float)(conf.Min*Math.Pow(conf.Multiplier, 2)));
                yield return new TestCaseData(conf, 40000).Returns(conf.Max);
            }
        }
        
        [TestCaseSource(nameof(Cases))]
        public float GetMaxSpeed_ReturnsExpected(SpeedMaxConf conf, float distance) {
            return BoatLogic.GetMaxSpeed(conf, distance);
        }
    }
}
