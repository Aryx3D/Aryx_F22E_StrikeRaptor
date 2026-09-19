using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;

namespace Aryx_F22E_StrikeRaptor
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Logger.LogInfo($"Aryx Dynamics {MyPluginInfo.PLUGIN_GUID} loaded. Await blueprinter start.");
            Harmony harmony = new Harmony("com.aryx.strikeraptor");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(WeaponMount), nameof(WeaponMount.Initialize))]
    public static class WeaponMountInitializePatch
    {
        private static void Prefix(WeaponMount __instance, out string __state)
        {
            __state = __instance.mountName;
        }

        private static void Postfix(WeaponMount __instance, string __state)
        {
            if (__instance?.prefab == null)
                return;

            if (__instance.prefab.GetComponent<AryxPreserveMountName>() == null)
                return;

            if (!string.IsNullOrEmpty(__state))
                __instance.mountName = __state;

        }
    }
    public sealed class AryxPreserveMountName : MonoBehaviour
    {
    }
}
