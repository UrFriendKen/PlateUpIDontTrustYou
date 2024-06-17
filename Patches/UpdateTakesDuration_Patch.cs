using HarmonyLib;
using Kitchen;
using System;
using System.Reflection;
using Unity.Entities;

namespace KitchenIDontTrustYou.Patches
{
    [HarmonyPatch]
    static class UpdateTakesDuration_Patch
    {
        static MethodBase TargetMethod()
        {
            Type type = AccessTools.FirstInner(typeof(UpdateTakesDuration), t => t.Name.Contains($"c__DisplayClass_OnUpdate_LambdaJob1"));
            return AccessTools.FirstMethod(type, method => method.Name.Contains("OriginalLambdaBody"));
        }

        [HarmonyPrefix]
        static void OriginalLambdaBody_Prefix(ref Entity e, ref CTakesDuration duration)
        {
            if (!Main.PrefManager.Get<bool>(Main.PREVENT_BLUEPRINT_PURCHASE_ID))
                return;

            Entity target = e;
            if (PatchController.StaticRequire(target, out CDurationInteractionProxy proxy))
                target = proxy.Proxy;

            if (!PatchController.StaticHas<CApplianceBlueprint>(target) ||
                !PatchController.StaticHas<CPurchaseAfterDuration>(target) ||
                !PatchController.StaticHas<CForSale>(target) ||
                !PatchController.StaticRequireBuffer(e, out DynamicBuffer<CBeingActedOnBy> beingActedOnBys))
                return;

            for (int i = 0; i < beingActedOnBys.Length; i++)
            {
                Entity interactor = beingActedOnBys[i].Interactor;
                if (interactor == default ||
                    !PatchController.StaticRequire(interactor, out CPlayer player))
                    continue;

                if (!Helpers.IsHost(player))
                {
                    duration.Active = false;
                    break;
                }
            }
        }
    }
}
