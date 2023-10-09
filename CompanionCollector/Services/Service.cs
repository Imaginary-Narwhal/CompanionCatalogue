using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace CompanionCollector.Services;
public class Service
{
    public static void Initialize(DalamudPluginInterface _pluginInterface)
    {
        _pluginInterface.Create<Service>();
    }

    [PluginService][RequiredVersion("1.0")] public static IChatGui ChatGui { get; private set; } = null;
    [PluginService][RequiredVersion("1.0")] public static ICommandManager CommandManager { get; private set; } = null;
    [PluginService][RequiredVersion("1.0")] public static IDataManager DataManager { get; private set; } = null;
    [PluginService][RequiredVersion("1.0")] public static IFramework Framework { get; private set; } = null;
    [PluginService][RequiredVersion("1.0")] public static DalamudPluginInterface Interface { get; private set; } = null;
    [PluginService][RequiredVersion("1.0")] public static IClientState ClientState { get; private set; } = null;
}
