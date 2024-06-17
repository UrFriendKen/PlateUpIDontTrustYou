using HarmonyLib;
using Kitchen;

namespace KitchenIDontTrustYou.Patches
{
    [HarmonyPatch]
    static class TriggerWorkshopCrafting_Patch
    {
        [HarmonyPatch(typeof(TriggerWorkshopCrafting), "IsPossible")]
        [HarmonyPrefix]
        static bool IsPossible_Prefix(ref InteractionData data, ref bool __result)
        {
            if (!Main.PrefManager.Get<bool>(Main.PREVENT_WORKSHOP_CRAFTING_ID))
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
