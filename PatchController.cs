using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace KitchenIDontTrustYou
{
    internal class PatchController : GameSystemBase, IModSystem
    {
        static PatchController _instance;

        protected override void Initialise()
        {
            base.Initialise();
            _instance = this;
        }

        protected override void OnUpdate()
        {
        }

        public static bool TryGetEntityQuery(out EntityQuery query, params ComponentType[] componentTypes)
        {
            if (_instance == null)
            {
                query = default;
                return false;
            }
            query = _instance.GetEntityQuery(componentTypes);
            return true;
        }


        public static bool TryGetEntityQuery(out EntityQuery query, NativeArray<ComponentType> componentTypes)
        {
            if (_instance == null)
            {
                query = default;
                return false;
            }
            query = _instance.GetEntityQuery(componentTypes);
            return true;
        }


        public static bool TryGetEntityQuery(out EntityQuery query, params EntityQueryDesc[] queryDesc)
        {
            if (_instance == null)
            {
                query = default;
                return false;
            }
            query = _instance.GetEntityQuery(queryDesc);
            return true;
        }

        public static bool StaticHas<T>() where T : struct, IComponentData
        {
            return _instance?.Has<T>() ?? false;
        }

        public static bool StaticHas<T>(Entity e) where T : struct, IComponentData
        {
            return _instance?.Has<T>(e) ?? false;
        }

        public static bool StaticRequire<T>(out T comp) where T : struct, IComponentData
        {
            comp = default;
            return _instance?.Require(out comp) ?? false;
        }

        public static bool StaticRequire<T>(Entity e, out T comp) where T : struct, IComponentData
        {
            comp = default;
            return _instance?.Require(e, out comp) ?? false;
        }

        public static bool StaticRequireBuffer<T>(Entity e, out DynamicBuffer<T> buffer) where T : struct, IBufferElementData
        {
            buffer = default;
            return _instance?.RequireBuffer(e, out buffer) ?? false;
        }

        public static bool StaticSet<T>(EntityQuery query, T comp) where T : struct, IComponentData
        {
            if (_instance == null)
                return false;

            using (NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp))
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    _instance.Set(entities[i], comp);
                }
            }
            return true;
        }

        public static bool StaticSet<T>(Entity e, T comp) where T : struct, IComponentData
        {
            if (_instance == null)
                return false;
            _instance.Set(e, comp);
            return true;
        }
    }
}
