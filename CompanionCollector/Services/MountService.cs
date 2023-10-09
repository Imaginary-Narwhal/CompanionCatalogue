using CompanionCollector.Models;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
//using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionCollector.Services;
public class MountProvider
{
    public List<Mount> AllMounts { get; set; } = new();
    public Lumina.Excel.ExcelSheet<Lumina.Excel.GeneratedSheets.Mount> RawMounts { get; set; }

    public MountProvider()
    {
        RawMounts = Service.DataManager.GetExcelSheet<Lumina.Excel.GeneratedSheets.Mount>();
        
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        foreach(var mount in RawMounts)
        {
            if (!string.IsNullOrWhiteSpace(mount.Singular) && mount.Icon > 0)
            {
                AllMounts.Add(new Mount
                {
                    Id = mount.RowId,
                    Name = textInfo.ToTitleCase(mount.Singular),
                    Icon = mount.Icon,
                    Owned = IsOwned(mount.RowId)
                });
            }
        }
    }

    private void UpdateOwned()
    {
        foreach(var mount in AllMounts)
        {
            mount.Owned = IsOwned(mount.Id);
        }
    }

    private unsafe bool IsOwned(uint Id)
    {
        return PlayerState.Instance()->IsMountUnlocked(Id);
    }

    public List<Mount> GetSortedMounts()
    {
        UpdateOwned();
        return AllMounts.OrderBy(x => !x.Owned).ThenBy(x => x.Id).ToList();
    }
}
