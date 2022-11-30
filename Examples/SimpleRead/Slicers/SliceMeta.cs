using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleRead.Slicers
{
    public class SliceMeta
    {
        public string Path { get; set; }

        public int Count { get; set; }

        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public int FactMinX { get; set; }
        public int FactMaxX { get; set; }
        public int FactMinY { get; set; }
        public int FactMaxY { get; set; }
        public int FactMinZ { get; set; }
        public int FactMaxZ { get; set; }

    }
}
