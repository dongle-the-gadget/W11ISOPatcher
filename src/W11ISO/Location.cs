using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W11ISO
{
    public class Location
    {
        public string OrigISO { get; }

        public string WorkingDir { get; }

        public string Product { get; }

        public Location(string orig, string working, string product)
        {
            OrigISO = orig;
            WorkingDir = working;
            Product = product;
        }
    }
}
