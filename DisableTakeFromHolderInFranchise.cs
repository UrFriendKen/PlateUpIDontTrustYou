using Kitchen;
using Unity.Entities;

namespace KitchenIDontTrustYou
{
    public class DisableTakeFromHolderInFranchise : GameSystemBase
    {
        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override void OnUpdate()
        {
            if (Has<SFranchiseMarker>() && (World.GetExistingSystem(typeof(TakeFromHolder)).Enabled || !World.GetExistingSystem(typeof(BlockCrateTakeFromHolder)).Enabled))
            {
                Main.LogInfo("Using BlockCrateTakeFromHolder");
                World.GetExistingSystem(typeof(TakeFromHolder)).Enabled = false;
                World.GetExistingSystem(typeof(BlockCrateTakeFromHolder)).Enabled = true;
            }
            else if (!Has<SFranchiseMarker>() && (!World.GetExistingSystem(typeof(TakeFromHolder)).Enabled || World.GetExistingSystem(typeof(BlockCrateTakeFromHolder)).Enabled))
            {
                Main.LogInfo("Using Vanilla TakeFromHolder");
                World.GetExistingSystem(typeof(TakeFromHolder)).Enabled = true;
                World.GetExistingSystem(typeof(BlockCrateTakeFromHolder)).Enabled = false;
            }
        }
    }
}
