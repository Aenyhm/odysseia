using Sources;
using Sources.Toolbox;
using UnityEngine.UI;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class CannonCooldownView : AbstractView {
        private Image _image;
        
        private void Awake() {
            _image = GetComponent<Image>();
        }
        
        public override void Render(GameState gameState, float dt) {
            var cooldown = gameState.PlayState.Cannon.ReloadCooldown;
            var cooldownMax = Services.Get<GameConf>().CannonConf.AmmoReloadTime;
            
            _image.fillAmount = cooldown/cooldownMax;
        }
    }
}
