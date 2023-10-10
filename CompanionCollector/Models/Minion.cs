using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionCollector.Models;
public class Minion
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public ushort Icon { get; set; }
    public bool Owned { get; set; }
}
