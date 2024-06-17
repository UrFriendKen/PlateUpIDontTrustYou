using Kitchen;
using KitchenMods;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;

namespace KitchenIDontTrustYou
{
    public class NonHostDetector : GenericSystemBase, IModSystem
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct SNonHostPresent : IComponentData, IModComponent { }

        EntityQuery Players;

        protected override void Initialise()
        {
            base.Initialise();
            Players = GetEntityQuery(typeof(CPlayer));
        }

        protected override void OnUpdate()
        {
            using (NativeArray<CPlayer> players = Players.ToComponentDataArray<CPlayer>(Allocator.Temp))
            {
                bool hasNonHost = false;
                for (int i = 0; i < players.Length; i++)
                {
                    if (!Helpers.IsHost(players[i]))
                    {
                        hasNonHost = true;
                        break;
                    }
                }

                if (!hasNonHost)
                    Clear<SNonHostPresent>();
                else if (!Has<SNonHostPresent>())
                    EntityManager.CreateEntity(typeof(SNonHostPresent), typeof(CDoNotPersist));
            }
        }
    }
}
