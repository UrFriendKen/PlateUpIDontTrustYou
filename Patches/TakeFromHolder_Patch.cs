using HarmonyLib;
using Kitchen;

namespace KitchenIDontTrustYou.Patches
{
    [HarmonyPatch]
    static class TakeFromHolder_Patch
    {
        [HarmonyPatch(typeof(TakeFromHolder), "IsPossible")]
        [HarmonyPrefix]
        static bool IsPossible_Prefix(ref InteractionData data, ref bool __result)
        {
            if (!Main.PrefManager.Get<bool>(Main.PREVENT_CRATE_PICK_UP_ID))
                return true;

            if (PatchController.StaticHas<SFranchiseMarker>() &&
                PatchController.StaticRequire(data.Interactor, out CPlayer player) &&
                !Helpers.IsHost(player) &&
                PatchController.StaticRequire(data.Target, out CItemHolder held) &&
                PatchController.StaticHas<CProvidesLoadoutItem>(held.HeldItem))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
