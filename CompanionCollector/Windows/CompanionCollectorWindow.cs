using CompanionCollector.Models;
using CompanionCollector.Services;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using ImGuiScene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CompanionCollector.Windows;
public sealed class CompanionCollectorWindow : Window, IDisposable
{
    private Mount SelectedMount { get; set; } = null;
    private Minion SelectedMinion { get; set; }= null;

    private readonly GameFontHandle TitleFont = Service.Interface.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamily.Axis, 25.0f));


    public CompanionCollectorWindow() : base(
            "Companion Collector", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        using (ImRaii.TabBar("Tabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton))
        {
            using (var mountTab = ImRaii.TabItem("Mounts"))
            {
                if (mountTab)
                {
                    DrawMounts();
                }
            }
            using (var minionTab = ImRaii.TabItem("Minions"))
            {
                if (minionTab)
                {
                    DrawMinions();
                }
            }
        }
    }

    public void DrawMounts()
    {
        var MountList = Service.MountProvider.GetSortedMounts();
        using (ImRaii.Child("mounts"))
        {
            ImGuiListClipperPtr Clipper;
            unsafe { Clipper = new(ImGuiNative.ImGuiListClipper_ImGuiListClipper()); }
            Clipper.Begin(MountList.Count);
            while (Clipper.Step())
            {
                for (int i = Clipper.DisplayStart; i < Clipper.DisplayEnd; i++)
                {
                    var mount = MountList[i];
                    var opacity = .5f;
                    var color = 0f;
                    if (SelectedMount != null)
                    {
                        if (mount.Id == SelectedMount.Id)
                        {
                            if (!SelectedMount.Owned)
                            {
                                color = 1f;
                            }
                            else
                            {
                                color = .3f;
                            }
                        }
                    }

                    using (ImRaii.PushColor(ImGuiCol.ChildBg, new Vector4(color, 0, color, opacity)))
                    {
                        using (var group = ImRaii.Child($"##{mount.Name}", new Vector2(300, 60), false, ImGuiWindowFlags.ChildWindow))
                        {
                            ImGui.SetCursorPos(new Vector2(
                                ImGui.GetCursorPosX() + 5,
                                ImGui.GetCursorPosY() + 6
                                ));
                            Service.DrawService.DrawIcon(mount.Icon, new Vector2(48, 48));
                            ImGui.SameLine();
                            using (ImRaii.PushFont(TitleFont.ImFont))
                            {
                                ImGui.TextWrapped($"{mount.Name}");
                            }
                            if (!mount.Owned)
                            {
                                using (ImRaii.PushColor(ImGuiCol.ChildBg, new Vector4(0, 0, 0, opacity + .25f)))
                                {
                                    ImGui.SetCursorPos(new Vector2(0, 0));
                                    using (var overlay = ImRaii.Child($"##over{mount.Name}", new Vector2(300, 60), false, ImGuiWindowFlags.ChildWindow))
                                    { }
                                    if (ImGui.IsItemClicked())
                                    {
                                        SelectedMount = mount;
                                    }
                                }
                            }
                        }
                        if (ImGui.IsItemClicked())
                        {
                            SelectedMount = mount;
                        }
                        if(ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left) && ImGui.IsItemHovered())
                        {
                            SelectedMount = mount;
                            SummonMount(mount.Id);
                        }
                    }
                }
            }
        }
    }

    public void DrawMinions()
    {
        var MinionList = Service.MinionProvider.GetSortedMinions();
        using (ImRaii.Child("minions"))
        {
            ImGuiListClipperPtr Clipper;
            unsafe { Clipper = new(ImGuiNative.ImGuiListClipper_ImGuiListClipper()); }
            Clipper.Begin(MinionList.Count);
            while (Clipper.Step())
            {
                for (int i = Clipper.DisplayStart; i < Clipper.DisplayEnd; i++)
                {
                    var minion = MinionList[i];
                    var opacity = .5f;
                    var color = 0f;
                    if (SelectedMinion != null)
                    {
                        if (minion.Id == SelectedMinion.Id)
                        {
                            if (!SelectedMinion.Owned)
                            {
                                color = 1f;
                            }
                            else
                            {
                                color = .3f;
                            }
                        }
                    }

                    using (ImRaii.PushColor(ImGuiCol.ChildBg, new Vector4(color, 0, color, opacity)))
                    {
                        using (var group = ImRaii.Child($"##{minion.Name}", new Vector2(300, 60), false, ImGuiWindowFlags.ChildWindow))
                        {
                            ImGui.SetCursorPos(new Vector2(
                                ImGui.GetCursorPosX() + 5,
                                ImGui.GetCursorPosY() + 6
                                ));
                            Service.DrawService.DrawIcon(minion.Icon, new Vector2(48, 48));
                            ImGui.SameLine();
                            using (ImRaii.PushFont(TitleFont.ImFont))
                            {
                                ImGui.TextWrapped($"{minion.Name}");
                            }

                            if (!minion.Owned)
                            {
                                using (ImRaii.PushColor(ImGuiCol.ChildBg, new Vector4(0, 0, 0, opacity + .25f)))
                                {
                                    ImGui.SetCursorPos(new Vector2(0, 0));
                                    using (var overlay = ImRaii.Child($"##over{minion.Name}", new Vector2(300, 60), false, ImGuiWindowFlags.ChildWindow))
                                    { }
                                    if (ImGui.IsItemClicked())
                                    {
                                        SelectedMinion = minion;
                                    }
                                }
                            }
                        }
                        if (ImGui.IsItemClicked())
                        {
                            SelectedMinion = minion;
                        }
                        if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left) && ImGui.IsItemHovered())
                        {
                            SelectedMinion = minion; ;
                            SummonMinion(minion.Id);
                        }
                    }
                }
            }
        }
    }

    private static unsafe void SummonMount(uint Id)
    {
        if(Id > 0)
        {
            ActionManager.Instance()->UseAction(ActionType.Mount, Id);
        }
    }

    private static unsafe void SummonMinion(uint Id)
    {
        if(Id > 0)
        {
            ActionManager.Instance()->UseAction(ActionType.Companion, Id);
        }
    }
}
