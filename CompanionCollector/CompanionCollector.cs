using CompanionCollector.Attributes;
using CompanionCollector.CommandHandler;
using CompanionCollector.Services;
using CompanionCollector.Windows;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CompanionCollector;
public sealed class Plugin : IDalamudPlugin
{
    public static string Name => "Companion Collector";
    private PluginCommandManager<Plugin> CommandManager { get; init; }
    internal CompanionCollectorWindow CompanionCollectorWindow { get; init; }

    public Plugin(DalamudPluginInterface _pluginInterface)
    {
        Service.Initialize(_pluginInterface);
        Service.DrawService = new();

        Service.ClientState.Login += () => 
        {
            Service.Configuration.Login(Service.ClientState.LocalPlayer.Name,
                Service.ClientState.LocalPlayer.HomeWorld.GameData.Name);
        };

        CommandManager = new(this);

        Service.Configuration = Service.Interface.GetPluginConfig() as Configuration ?? new Configuration();

        CompanionCollectorWindow = new CompanionCollectorWindow();
        Service.WindowSystem.AddWindow(CompanionCollectorWindow);

        Service.Interface.UiBuilder.Draw += DrawUI;

        Service.MountProvider = new();

    }

    public void Dispose()
    {
        CommandManager.Dispose();
        Service.WindowSystem.RemoveAllWindows();
        Service.Interface.UiBuilder.Draw -= DrawUI;
        Service.DrawService.textureDictionary.Clear();
    }

    private void DrawUI()
    {
        Service.WindowSystem.Draw();
    }

    [Command("/pcc")]
    [HelpMessage("Open the Companion Collector window")]
    public void ItemCounterCommand(string _, string __)
    {
        CompanionCollectorWindow.IsOpen = true;

        File.WriteAllText(@"c:\temp\mounts.json", JsonConvert.SerializeObject(Service.MountProvider.AllMounts, Formatting.Indented));
    }
}
