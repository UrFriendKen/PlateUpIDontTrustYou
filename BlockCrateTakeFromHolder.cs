using Kitchen;

namespace KitchenIDontTrustYou
{
    public class BlockCrateTakeFromHolder : TakeFromHolder
    {
        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            if (Require(data.Interactor, out CPlayer player) && !Helpers.IsHost(player) && Require(data.Target, out CItemHolder held) && Has<CProvidesLoadoutItem>(held.HeldItem))
            {
                return false;
            }
            
            return base.IsPossible(ref data);
        }

        protected override void Perform(ref InteractionData data)
        {
            base.Perform(ref data);
        }
    }
}
