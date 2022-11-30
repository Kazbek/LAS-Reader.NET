using SimpleRead.Slicers;

namespace OctetProcessor
{
    public class OctetTreeProducer
    {
        public static List<OctetTree> GetTrees(SliceMeta meta, List<PointWithColor> points)
        {
            int size = meta.MaxX - meta.MinX;

            List<OctetTree> results;

            if (meta.FactMaxZ - meta.FactMinZ + 1 <= size)
            {
                results = new List<OctetTree> { new OctetTree(points, meta.MinX, meta.MaxX, meta.MinY, meta.MaxY, meta.FactMinZ, meta.FactMinZ + size) };
            }
            else
            {
                int col = (int)Math.Ceiling((meta.FactMaxZ - meta.FactMinZ + 1d) / size);
                results = new List<OctetTree>(col);
                for (int i = 0; i < col; i++)
                {
                    int zMin = meta.FactMinZ + size * i;
                    int zMax = meta.FactMinZ + size * (i + 1);
                    var pts = points.Where(p => zMin <= p.Z && p.Z < zMax).ToList();
                    if(pts.Count > 0)
                        results.Add(new OctetTree(pts, meta.MinX, meta.MaxX, meta.MinY, meta.MaxY, zMin, zMax));
                }
            }

            return results;
        }
    }
}
