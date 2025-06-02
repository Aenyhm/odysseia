using Sources;
using Sources.Core;
using Sources.Toolbox;
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
            CannonConf = new CannonConf {
                AmmoReloadTime = 3f,
                AmmoSpawnFreq = 2f,
                AmmoSpeed = 30f,
                AmmoMax = 10,
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
            RelicConf = new RelicConf {
                Distance = 800,
                CoinValue = 100,
                Score = 500
            },
            WindConf = new WindConf {
                ChangeFreq = new RangeI32 { Min = 2, Max = 5 },
                ChangeDuration = 1,
                AngleMax = 30,
            },
            
            EnableBoatCollisions = true,
        };
    }
}
