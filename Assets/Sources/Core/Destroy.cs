namespace Sources.Core {
    public static class DestroySystem {
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            var entitiesByType = playState.Region.EntitiesByType;
            
            // Note: C'est pas génial ce cas particulier, il faudrait le gérer autrement (dans le fichier de la sirène).
            foreach (var mermaid in entitiesByType[EntityType.Mermaid]) {
                if (mermaid.Destroy && boat.CharmedById == mermaid.Id) {
                    boat.CharmedById = 0;
                }
            }

            foreach (var entities in entitiesByType.Values) {
                entities.RemoveAll(e => e.Destroy);
            }
        }
    }
}
