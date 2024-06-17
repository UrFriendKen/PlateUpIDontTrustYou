using HarmonyLib;
using KitchenMods;
using PreferenceSystem;
using System.Reflection;
using UnityEngine;

namespace KitchenIDontTrustYou
{
    public class Main : IModInitializer
    {
        public const string MOD_GUID = "IcedMilo.PlateUp.IDontTrustYou";
        public const string MOD_NAME = "I Don't Trust You";
        public const string MOD_VERSION = "0.2.2";
        public const string MOD_AUTHOR = "IcedMilo";
        public const string MOD_GAMEVERSION = ">=1.1.1";

        internal static PreferenceSystemManager PrefManager;

        internal const string MULTIPLAYER_LOCK_SCRAPPER_ID = "multiplayerLockScrapper";
        internal const string PREVENT_WORKSHOP_CRAFTING_ID = "preventWorkshopCrafting";
        internal const string PREVENT_CRATE_PICK_UP_ID = "preventCratePickUp";
        internal const string PREVENT_BLUEPRINT_PURCHASE_ID = "preventBlueprintPurchase";
        internal const string PREVENT_FRANCHISE_SELECT_ID = "preventFranchiseSelect";
        internal const string PREVENT_NO_CLIP_ID = "preventNoClip";

        public Main()
        {
            Harmony harmony = new Harmony(MOD_GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void PostActivate(Mod mod)
        {
            // For log file output so the official plateup support staff can identify if/which a mod is being used
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");


            PrefManager = new PreferenceSystemManager(MOD_GUID, MOD_NAME);
            PrefManager
                .AddLabel("Lock Franchise Scrapper")
                .AddOption<bool>(
                    MULTIPLAYER_LOCK_SCRAPPER_ID,
                    true,
                    new bool[] { false, true },
                    new string[] { "Never", "When non-host present" })
                .AddLabel("Non-host Workshop Crafting")
                .AddOption<bool>(
                    PREVENT_WORKSHOP_CRAFTING_ID,
                    true,
                    new bool[] { true, false },
                    new string[] { "Disallowed", "Allowed" })
                .AddLabel("Non-host Workshop Crate Pickup")
                .AddOption<bool>(
                    PREVENT_CRATE_PICK_UP_ID,
                    true,
                    new bool[] { true, false },
                    new string[] { "Disallowed", "Allowed" })
                .AddLabel("Non-host Blueprint Purchase")
                .AddOption<bool>(
                    PREVENT_BLUEPRINT_PURCHASE_ID,
                    true,
                    new bool[] { true, false },
                    new string[] { "Disallowed", "Allowed" })
                .AddLabel("Non-host Select Franchise")
                .AddOption<bool>(
                    PREVENT_FRANCHISE_SELECT_ID,
                    true,
                    new bool[] { true, false },
                    new string[] { "Disallowed", "Allowed" })
                .AddLabel("No Clip")
                .AddOption<bool>(
                    PREVENT_NO_CLIP_ID,
                    true,
                    new bool[] { true, false },
                    new string[] { "Disallowed", "Allowed" })
                .AddSpacer()
                .AddSpacer();

            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.MainMenu);
            PrefManager.RegisterMenu(PreferenceSystemManager.MenuType.PauseMenu);
        }

        public void PreInject()
        {

        }

        public void PostInject()
        {
            Helpers.InitFullColliderAppliances();
        }

        #region Logging
        // You can remove this, I just prefer a more standardized logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }

}
