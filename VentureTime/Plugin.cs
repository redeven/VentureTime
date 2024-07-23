using System;
using System.Linq;
using System.Timers;
using Dalamud.Plugin;
using Dalamud.Game.Gui.Dtr;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using FFXIVClientStructs.FFXIV.Client.Game;
using VentureTime.Utils;

namespace VentureTime;

public sealed class Plugin : IDalamudPlugin
{
    private RetainerInfo[]? retainers;
    private byte? retainerCount;
    private readonly IDtrBarEntry dtrEntry;
    private readonly Timer dtrTextTimer = new Timer(1000);

    public Plugin(IDalamudPluginInterface pluginInterface)
    {
        pluginInterface.Create<Svc>();

        dtrEntry = Svc.DtrBar.Get("VentureTime");
        dtrEntry.Text = $"Ventures: ?/?";
        dtrEntry.Tooltip = "Interact with a Summoning Bell to initialize data";
        dtrEntry.Shown = true;

        CheckRetainerInfo();
        UpdateDtrText();

        Svc.AddonLifecycle.RegisterListener(AddonEvent.PostRequestedUpdate, "RetainerList", OnRetainerList);
        dtrTextTimer.Elapsed += OnTimerElapsed;
        dtrTextTimer.Start();
    }

    public void Dispose()
    {
        dtrEntry.Remove();
        Svc.AddonLifecycle.UnregisterListener(OnRetainerList);
        dtrTextTimer?.Stop();
    }

    private void OnRetainerList(AddonEvent type, AddonArgs args)
    {
        CheckRetainerInfo();
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        UpdateDtrText();
    }

    private unsafe void CheckRetainerInfo()
    {
        if (Svc.ClientState.LocalPlayer == null)
            return;

        var manager = RetainerManager.Instance();
        if (manager == null || manager->Ready != 1)
            return;

        var retainerList = manager->Retainers;
        retainerCount = manager->GetRetainerCount();
        retainers = RetainerInfo.GenerateDefaultArray();
        for (byte i = 0; i < retainerCount; ++i)
        {
            var retainer = new RetainerInfo(retainerList[i]);
            retainers[i] = retainer;
        }
    }

    private void UpdateDtrText()
    {
        if (retainers != null)
        {
            var _retainers = retainers.Where((retainer) => retainer.RetainerId != 0);
            var pendingVentures = _retainers.Where((retainer) => retainer.Venture > DateTime.UtcNow).Select((retainer) => retainer.Venture).ToList();

            if (pendingVentures.Count > 0)
            {
                var upcomingVenture = pendingVentures.Min();
                dtrEntry.Text = $"Ventures: {pendingVentures.Count}/{_retainers.Count()} ({Helpers.FormatTimeSpan(Helpers.TimeLeftFromNow(upcomingVenture))})";
                dtrEntry.Tooltip = null;
            }
            else
            {
                dtrEntry.Text = $"Ventures: 0/{_retainers.Count()}";
                dtrEntry.Tooltip = null;
            }
        }
    }
}
