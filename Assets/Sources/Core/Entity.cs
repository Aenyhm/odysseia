using System;
using Sources.Toolbox;

namespace Sources.Core {
    
    [Serializable]
    public struct Transform {
        public Vec3F32 position;
        public Vec3F32 rotation;
        public Vec3F32 size;
    }
    
    public abstract class Entity {
        public int id;
        public Transform transform;
        public Vec3F32 color;
        
        // Ne pas utiliser de constructeur sur des classes sérialisées pour éviter des comportements fantômes.
        
        public override int GetHashCode() => id;
    }
    
    public static class EntityManager {
        private static int currentId;

        public static E Create<E>() where E : Entity, new() {
            var e = new E();
            Reset(e);
            return e;
        }
        
        public static void Reset(Entity e) {
            e.id = ++currentId;
            e.transform = new Transform();
            e.transform.size = Vec3F32.one;
            e.color = Vec3F32.one;
        }
    }
}
