using Dalamud.IoC;
using Dalamud.Plugin.Services;

namespace VentureTime
{
    internal class Svc
    {
        [PluginService] internal static IDtrBar DtrBar { get; set; } = null!;
        [PluginService] internal static IClientState ClientState { get; set; } = null!;
        [PluginService] internal static IAddonLifecycle AddonLifecycle { get; set; } = null!;
    }
}
