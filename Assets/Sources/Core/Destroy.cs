namespace Sources.Core {
    public static class DestroySystem {
        public static void Execute(ref GameState gameState) {
            ref var playState = ref gameState.PlayState;
            ref var entities = ref playState.Region.Entities;
            
            foreach (var e in entities) {
                if (e.Destroy) {
                    switch (e.Type) {
                        case EntityType.Mermaid: {
                            if (playState.Boat.CharmedById == e.Id) playState.Boat.CharmedById = 0;
                        } break;
                    }
                }
            }
            
            gameState.PlayState.Region.Entities.RemoveAll(e => e.Destroy);
        }
    }
}
