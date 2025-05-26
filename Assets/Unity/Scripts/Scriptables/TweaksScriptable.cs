using Sources;
using Sources.Core;
using UnityEngine;

namespace Unity.Scripts.Scriptables {
    
    [CreateAssetMenu(fileName = "Tweaks", menuName = "Scriptable Objects/Tweaks")]
    public class TweaksScriptable : ScriptableObject {
        public GameConf GameConf = new() {
            BoatConf = new BoatConf {
                SpeedMaxConf = new SpeedMaxConf { DistanceStep = 500, Multiplier = 1.1f, Min = 20, Max = 40 },
                SailConf = new SailConf { AngleMax = 30f, WindwardAngleRange = 20f },
                PositionY = 0.5f,
                SpeedCollisionFactor = 0.5f,
                SpeedZStart = 10,
                SpeedZMin = 10,
                SpeedX = 30,
                HealthMax = 3,
            },
            CoinConf = new CoinConf {
                CoinDistance = 1.5f, // 10 coins = 15m
                CoinLineCount = 10,
                CoinLineBonus = 10,
            },
            RegionConf = new RegionConf {
                ZenDistance = 100,
                RegionDistance = 1000,
                PortalCount = 2,
            },
            WindConf = new WindConf {
                AngleMax = 30,
                ChangeDistance = 60
            },
        };
    }
}
