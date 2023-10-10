using CompanionCollector.Models;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CompanionCollector.Services;
public class MinionProvider
{
    public List<Minion> Minions { get; set; } = new();
    public ExcelSheet<Companion> RawMinions { get; set; }

    //Special IDs for minions that are summoned throught a single Minion
    //Minion of Light and Wind-Up Leaders
    private List<uint> SpecialMinions = new() 
    {
        68,
        69,
        70,
        72,
        73,
        74
    };

    public MinionProvider()
    {
        RawMinions = Service.DataManager.GetExcelSheet<Companion>();

        var textInfo = new CultureInfo("en-US", false).TextInfo;

        foreach(var minion in RawMinions)
        {
            //Check for the minions that have multiple minions on one object
            if(SpecialMinions.Contains(minion.RowId))
            {
                continue;
            }
            if(!string.IsNullOrWhiteSpace(minion.Singular) && minion.Icon > 0)
            {
                Minions.Add(new Minion
                {
                    Id = minion.RowId,
                    Name = textInfo.ToTitleCase(minion.Singular),
                    Icon = minion.Icon,
                    Owned = IsOwned(minion.RowId)
                });
            }
        }
    }
    private void UpdateOwned()
    {
        foreach (var minion in Minions)
        {
            minion.Owned = IsOwned(minion.Id);
        }
    }

    private unsafe bool IsOwned(uint Id)
    {
        return UIState.Instance()->IsCompanionUnlocked(Id);
    }

    public List<Minion> GetSortedMinions()
    {
        UpdateOwned();
        return Minions.OrderBy(x => !x.Owned).ThenBy(x => x.Id).ToList();
    }
}
