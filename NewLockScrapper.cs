using Kitchen;
using Unity.Collections;
using Unity.Entities;

namespace KitchenIDontTrustYou
{
    public class NewLockScrapper : LockScrapper
    {
        EntityQuery Scrappers;
        EntityQuery Players;

        protected override void Initialise()
        {
            base.Initialise();
            Scrappers = GetEntityQuery(typeof(CFranchiseScrapper));
            Players = GetEntityQuery(typeof(CPlayer));
        }

        protected override void OnUpdate()
        {
            using (NativeArray<CPlayer> players = Players.ToComponentDataArray<CPlayer>(Allocator.Temp))
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (!Helpers.IsHost(players[i]))
                    {
                        base.EntityManager.AddComponent<CPreventUse>(Scrappers);
                        return;
                    }
                }
            }
            base.OnUpdate();
        }
    }
}
