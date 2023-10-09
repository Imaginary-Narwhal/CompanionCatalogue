using CompanionCollector.Models;
using Dalamud.Configuration;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin;
using LString = Lumina.Text.SeString;
using System;
using System.Collections.Generic;
using System.Linq;
using CompanionCollector.Services;

namespace CompanionCollector;
[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    internal List<Character> Characters { get; set; } = new();

#nullable enable
    // the below exist just to make saving less cumbersome
    [NonSerialized] internal Character CurrentCharacter = new();
#nullable disable

    public void Save()
    {
        Service.Interface.SavePluginConfig(this);
    }

    public void Login(SeString _name, LString _world)
    {
        var chara = Characters.FirstOrDefault(x => x.Name == _name.ToString() && x.World == _world.ToString());
        if (chara != null)
        {
            CurrentCharacter = chara;

        }
        else
        {
            CurrentCharacter = new()
            {
                Name = _name.ToString(),
                World = _world.ToString()
            };

            Characters.Add(CurrentCharacter);
            Save();
        }
    }
}
