using System;
using Sources.Toolbox;

namespace Sources.Core {
    [Serializable]
    public class Boat : Entity {
        public Vec3F32 velocity;
        public float speed;
        public int lane;
    }

    public static class BoatController {
        private const float SPEED_MIN = 1f;
        private const float SPEED_MAX = 30f;
        
        public static Boat Create() {
            var boat = EntityManager.Create<Boat>();
            boat.speed = 200f;
            boat.transform.position.y = 2f;
            boat.velocity.z = boat.speed;
            boat.transform.size = Vec3F32.one*0.2f;
            
            Services.Get<Platform>().renderer.Create(boat);
            
            return boat;
        }

        public static void Update(InputState inputState, float dt) {
            var gs = Services.Get<GameState>();

            var boat = gs.boat;
            
            CheckHorizontalMove(boat, inputState);

            boat.transform.position += boat.velocity*dt;
        }
        
        private static void CheckHorizontalMove(Boat boat, InputState inputState) {
            if (boat.velocity.x == 0f) {
                var deltaX = Convert.ToInt32(inputState.Horizontal);
                var targetLane = boat.lane + deltaX;

                if (targetLane is >= -1 and <= 1) {
                    boat.lane = targetLane;
                    boat.velocity.x = deltaX*boat.speed; // FIXME: possibilité de changer la vitesse en cours de changement de lane
                }
            } else {
                var targetX = boat.lane*SceneryController.LANE_DISTANCE;

                var moveLaneCompleted = (
                    boat.velocity.x < 0f && boat.transform.position.x <= targetX ||
                    boat.velocity.x > 0f && boat.transform.position.x >= targetX
                );
                if (moveLaneCompleted) {
                    boat.velocity.x = 0f;
                    boat.transform.position.x = targetX;
                }
            }
        }
    }
}
