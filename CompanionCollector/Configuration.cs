using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace CompanionCollector;
[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

#nullable enable
    // the below exist just to make saving less cumbersome
    [NonSerialized]
    private DalamudPluginInterface? PluginInterface;
#nullable disable

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        this.PluginInterface = pluginInterface;
    }

    public void Save()
    {
        this.PluginInterface!.SavePluginConfig(this);
    }
}
