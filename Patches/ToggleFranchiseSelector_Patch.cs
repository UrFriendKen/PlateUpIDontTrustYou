using HarmonyLib;
using Kitchen;

namespace KitchenIDontTrustYou.Patches
{
    [HarmonyPatch]
    static class ToggleFranchiseSelector_Patch
    {
        [HarmonyPatch(typeof(ToggleFranchiseSelector), "IsPossible")]
        [HarmonyPrefix]
        static bool IsPossible_Prefix(ref InteractionData data, ref bool __result)
        {
            if (!Main.PrefManager.Get<bool>(Main.PREVENT_FRANCHISE_SELECT_ID))
                return true;

            //Check if is host
            if (!PatchController.StaticRequire(data.Interactor, out CPlayer player) || !Helpers.IsHost(player))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
