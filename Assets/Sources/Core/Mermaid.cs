using System;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public struct MermaidConf {
        public float PowerReloadDuration;
        public float StunDuration;
        public int SightDistance;
    }
    
    [Serializable]
    public struct MermaidData {
        public float PowerReloadCooldown;
        public float StunCooldown;
    }
    
    public static class MermaidSystem {
        public static void Execute(GameState gameState) {
            var mermaids = gameState.PlayState.Region.EntitiesByType[EntityType.Mermaid];
            ref var boat = ref gameState.PlayState.Boat;
            
            var mermaidConf = Services.Get<GameConf>().MermaidConf;

            for (var i = 0; i < mermaids.Count; i++) {
                ref var e = ref mermaids.Items[i];
 
                if (boat.CharmedById == e.Id) {
                    // Désenvoûte une fois le temps de stun passé.
                    e.MermaidData.StunCooldown = Math.Max(0, e.MermaidData.StunCooldown - Clock.DeltaTime);
                    if (e.MermaidData.StunCooldown == 0) {
                        boat.CharmedById = 0;
                    }
                }

                if (e.MermaidData.PowerReloadCooldown > 0) {
                    e.MermaidData.PowerReloadCooldown = Math.Max(0, e.MermaidData.PowerReloadCooldown - Clock.DeltaTime);
                } else if (boat.CharmedById == 0) {
                    if (e.Position.Z > boat.Position.Z && e.Position.Z - boat.Position.Z < mermaidConf.SightDistance) {
                        // Envoûtement
                        e.MermaidData.PowerReloadCooldown = mermaidConf.PowerReloadDuration;
                        e.MermaidData.StunCooldown = mermaidConf.StunDuration;
                        boat.CharmedById = e.Id;
                    }
                }
            }
        }
    }
}
