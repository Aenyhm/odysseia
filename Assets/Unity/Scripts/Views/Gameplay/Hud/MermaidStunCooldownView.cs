using Sources;
using Sources.Core;
using Sources.Toolbox;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Scripts.Views.Gameplay.Hud {
    public class MermaidStunCooldownView : AbstractView {
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private GameObject _parent;

        public override void Render(GameState gameState, float dt) {
            var boat = gameState.PlayState.Boat;
            
            var show = boat.CharmedById != 0;
            
            _parent.SetActive(show);
            
            if (show) {
                var mermaids = gameState.PlayState.Region.EntitiesByType[EntityType.Mermaid];
                var mermaidConf = Services.Get<GameConf>().MermaidConf;
                
                foreach (var e in mermaids) {
                    if (boat.CharmedById == e.Id) {
                        _cooldownImage.fillAmount = e.MermaidData.StunCooldown/mermaidConf.StunDuration;
                        break;
                    }
                }
            }
        }
    }
}
