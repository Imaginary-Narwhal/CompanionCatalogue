using CompanionCollector.Attributes;
using CompanionCollector.CommandHandler;
using CompanionCollector.Services;
using CompanionCollector.Windows;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;

#nullable disable

namespace CompanionCollector;
public sealed class Plugin : IDalamudPlugin
{
    public static string Name => "Companion Collector";

    internal Configuration Config { get; init; }
    public WindowSystem WindowSystem = new("CompanionCollector");

    private PluginCommandManager<Plugin> CommandManager { get; init; }

    private CompanionCollectorWindow CompanionCollectorWindow { get; init; }

    public Plugin(DalamudPluginInterface _pluginInterface)
    {
        Service.Initialize(_pluginInterface);
        CommandManager = new(this);

        Config = Service.Interface.GetPluginConfig() as Configuration ?? new Configuration();
        Config.Initialize(Service.Interface);

        CompanionCollectorWindow = new CompanionCollectorWindow(this);
        WindowSystem.AddWindow(CompanionCollectorWindow);

        Service.Interface.UiBuilder.Draw += DrawUI;
    }

    public void Dispose()
    {
        CommandManager.Dispose();
        WindowSystem.RemoveAllWindows();
        Service.Interface.UiBuilder.Draw -= DrawUI;
    }

    private void DrawUI()
    {
        WindowSystem.Draw();
    }

    [Command("/pcc")]
    [HelpMessage("Open the Companion Collector window")]
    public void ItemCounterCommand(string _, string __)
    {
        CompanionCollectorWindow.IsOpen = true;
        Service.ChatGui.Print("Open teh window.");
    }
}
