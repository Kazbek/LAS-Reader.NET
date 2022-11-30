using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctetProcessor
{
    public class OctetTree
    {
        //public int Level = 0;
        public PointWithColor Root { get; set; }
        public OctetTree[,,] Nodes = new OctetTree[2,2,2];
        public OctetTree(List<PointWithColor> points, int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            int x = (maxX + minX) / 2;
            int y = (maxY + minY) / 2;
            int z = (maxZ + minZ) / 2;

            //if (maxZ - minZ > maxX - minX || maxZ - minZ > maxY - minY)
            //    throw new ArgumentException($"Wrong size x:{maxX - minX} y:{maxY - minY} z:{maxZ - minZ}");

            List<PointWithColor>[,,] subPoints = new List<PointWithColor>[2, 2, 2];
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        subPoints[i, j, k] = new List<PointWithColor>();

            Root = points[0];
            int dist = Root.SqrDistance(new PointWithColor { X = x, Y = y, Z = z });
            for(int i = 1; i < points.Count; i++)
            {
                var point = points[i];
                PointWithColor toPut;
                if (Root.SqrDistance(point) >= dist)
                {
                    toPut = point;
                }
                else
                {
                    toPut = Root;
                    Root = point;
                }

                subPoints[
                    toPut.X < x ? 0 : 1,
                    toPut.Y < y ? 0 : 1,
                    toPut.Z < z ? 0 : 1
                    ].Add(toPut);

            }

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        if(subPoints[i, j, k].Count > 0)
                        {
                            int x1 = i == 0 ? minX : x;
                            int x2 = i == 0 ? x : maxX;

                            int y1 = i == 0 ? minY : y;
                            int y2 = i == 0 ? y : maxY;

                            int z1 = i == 0 ? minZ : z;
                            int z2 = i == 0 ? z : maxZ;

                            Nodes[i, j, k] = new OctetTree(subPoints[i, j, k], x1, x2, y1, y2, z1, z2);
                        }
        }

        public Dictionary<int, List<PointWithColor>> GetDataByLevels()
        {
            Dictionary<int, List<PointWithColor>> data = new Dictionary<int, List<PointWithColor>>();
            AddDataByLevels(data, 0);
            return data;
        }

        private void AddDataByLevels(Dictionary<int, List<PointWithColor>> data, int level)
        {
            if(data.TryGetValue(level, out List<PointWithColor> list))
            {
                list.Add(Root);
            }
            else
            {
                data.Add(level, new List<PointWithColor> { Root });
            }

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < 2; k++)
                        if (Nodes[i, j, k] != null)
                            Nodes[i, j, k].AddDataByLevels(data, level + 1);
        }
    }
}
