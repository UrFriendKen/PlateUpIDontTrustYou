using Kitchen;

namespace KitchenIDontTrustYou
{
    public class NewToggleFranchiseSelector : ToggleFranchiseSelector
    {
        protected override void Initialise()
        {
            base.Initialise();
        }

        protected override bool IsPossible(ref InteractionData data)
        {
            //Check if is host
            if (!Require(data.Interactor, out CPlayer player) || !Helpers.IsHost(player))
            {
                return false;
            }
            return base.IsPossible(ref data);
        }
    }
}
