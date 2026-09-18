using BepInEx;
using BepInEx.Logging;

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
        }
    }
}
