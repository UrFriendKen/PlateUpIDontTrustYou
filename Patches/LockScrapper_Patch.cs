using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace KitchenIDontTrustYou.Patches
{
    [HarmonyPatch]
    static class LockScrapper_Patch
    {
        static bool _isInit = false;
        static bool _hasEntityQuery = false;
        static EntityQuery _scrappers;

        [HarmonyPatch(typeof(LockScrapper), "OnUpdate")]
        [HarmonyPrefix]
        static bool OnUpdate_Prefix()
        {
            if (!Main.PrefManager.Get<bool>(Main.MULTIPLAYER_LOCK_SCRAPPER_ID))
                return true;

            if (!_isInit)
            {
                _hasEntityQuery = PatchController.TryGetEntityQuery(out _scrappers, typeof(CFranchiseScrapper));
                if (_hasEntityQuery)
                    _isInit = true;
            }

            if (!_hasEntityQuery)
                return true;

            //Check if is host
            if (PatchController.StaticHas<NonHostDetector.SNonHostPresent>())
            {
                PatchController.StaticSet(_scrappers, default(CPreventUse));
                return false;
            }
            return true;
        }
    }
}
