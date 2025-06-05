namespace Sources.Core {
    public static class DestroySystem {
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            ref var boat = ref playState.Boat;
            var entities = playState.Region.Entities;
            
            foreach (var e in entities) {
                if (e.Destroy) {
                    switch (e.Type) {
                        case EntityType.Mermaid: {
                            if (boat.CharmedById == e.Id) boat.CharmedById = 0;
                        } break;
                    }
                }
            }
            
            entities.RemoveAll(e => e.Destroy);
        }
    }
}
