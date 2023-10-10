using CompanionCollector.Models;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Lumina.Excel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sheets = Lumina.Excel.GeneratedSheets;

namespace CompanionCollector.Services;
public class MountProvider
{
    public List<Mount> Mounts { get; set; } = new();
    public ExcelSheet<Sheets.Mount> RawMounts { get; set; }

    public MountProvider()
    {
        RawMounts = Service.DataManager.GetExcelSheet<Sheets.Mount>();
        
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        foreach(var mount in RawMounts)
        {
            if (!string.IsNullOrWhiteSpace(mount.Singular) && mount.Icon > 0)
            {
                Mounts.Add(new Mount
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
        foreach(var mount in Mounts)
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
        return Mounts.OrderBy(x => !x.Owned).ThenBy(x => x.Id).ToList();
    }
}