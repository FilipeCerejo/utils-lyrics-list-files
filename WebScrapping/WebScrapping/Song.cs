using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapping;
public class Song
{
    public string? Name { get; set; } = "NNN";
    public int Number { get; set; } = 0;
    public string? Url { get; set; } = "UUU";
    public StringBuilder Lyrics { get; set; } = new();
    public string Title
    {
        get
        {
            return $"{string.Format("{0:D3}", Number)} - {Name}";
        }
    }
    public override string ToString()
    {
        return $"{Number}: {Name} ({Lyrics?.Length} chars). {Url}";
    }
}
