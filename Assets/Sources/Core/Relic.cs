using System;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public struct RelicConf {
        public int Distance;
        public int CoinValue;
        public int Score;
    }
    
    public static class RelicLogic {
        public static Entity Create() {
            var relicConf = Services.Get<GameConf>().RelicConf;
            
            var e = new Entity();
            e.Id = EntityLogic.NextId;
            e.Type = EntityType.Relic;
            
            var lane = Enums.GetRandom<LaneType>();
            
            e.Coords = new[] { new Vec2I32((int)lane, relicConf.Distance/CoreConfig.GridScale) };
            e.Position = EntityLogic.GetPosition(EntityType.Relic, e.Coords);
            
            return e;
        }
    }
    
    public static class RelicSystem {
        public static void Execute(GameState gameState) {
            ref var playState = ref gameState.PlayState;
            var relics = playState.Region.EntitiesByType[EntityType.Relic];
            var boat = playState.Boat;
            
            var boxes = Services.Get<RendererConf>().BoundingBoxesByEntityType;
            var relicConf = Services.Get<GameConf>().RelicConf;

            for (var i = 0; i < relics.Count; i++) {
                ref var e = ref relics.Items[i];
                
                if (Collisions.CheckCollisionBoxes(e.Position, boxes[e.Type], boat.Position, boxes[EntityType.Boat])) {
                    ScoreLogic.Add(gameState, relicConf.Score);
                    playState.CoinCount += relicConf.CoinValue;
                    e.Destroy = true;
                }
            }
        }
    }
}
