using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CompassView : AbstractView {
        [SerializeField] private GameObject _compass;
        [SerializeField] private GameObject _windwardRange;
        [SerializeField] private GameObject _sailDirection;
        [SerializeField] private float _angleFactor = 1.5f;
        [SerializeField] private Color _rangeColorIn;
        [SerializeField] private Color _rangeColorOut;

        private float _windwardRangeInitialAngle;
        private Image _windwardRangeImage;

        private void Awake() {
            _windwardRangeInitialAngle = _windwardRange.transform.localEulerAngles.z;
            _windwardRangeImage = _windwardRange.GetComponent<Image>();
        }
        
        public override void Render(GameState gameState, float dt) {
            var wind = gameState.PlayState.Wind;
            var boat = gameState.PlayState.Boat;
            

            _compass.transform.RotateOnAxis(Axis.Z, -wind.CurrentAngle*_angleFactor);
            _sailDirection.transform.RotateOnAxis(Axis.Z, -boat.SailAngle*_angleFactor);
            
            var sailConf = Services.Get<GameConf>().BoatConf.SailConf;
            var windwardAngle = BoatLogic.GetWindwardAngle(sailConf, wind.CurrentAngle);
            var windwardCenter = (windwardAngle.X + windwardAngle.Y)/2;
            _windwardRange.transform.RotateOnAxis(Axis.Z, -windwardCenter*_angleFactor + _windwardRangeInitialAngle);
   
            _windwardRangeImage.color = boat.SailWindward ? _rangeColorIn : _rangeColorOut;
        }
    }
}
