using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace VentureTime
{
    internal class Svc
    {
        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; set; } = null!;
        [PluginService] internal static IDtrBar DtrBar { get; set; } = null!;
        [PluginService] internal static IClientState ClientState { get; set; } = null!;
        [PluginService] internal static IPluginLog Log { get; set; } = null!;
        [PluginService] internal static IDataManager GameData { get; set; } = null!;
        [PluginService] internal static IFramework Framework { get; set; } = null!;
        [PluginService] internal static IAddonLifecycle AddonLifecycle { get; set; } = null!;
    }
}
