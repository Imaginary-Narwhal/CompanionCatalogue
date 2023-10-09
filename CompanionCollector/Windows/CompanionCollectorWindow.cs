using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using System;
using System.Numerics;

namespace CompanionCollector.Windows;
public sealed class CompanionCollectorWindow : Window, IDisposable
{
    private readonly Plugin Plugin;

    public CompanionCollectorWindow(Plugin plugin) : base(
            "CompanionCollector", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.Plugin = plugin;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        ImGui.Text($"The random config bool is {this.Plugin.Config.SomePropertyToBeSavedAndWithADefault}");
    }
}
