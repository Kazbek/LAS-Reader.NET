using SimpleRead;
using SimpleRead.Slicers;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;

namespace OctetProcessor
{
    internal class Program
    {
        static string LeveledFolder = "Leveled";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            SliceGeneral sg = JsonSerializer.Deserialize<SliceGeneral>(File.ReadAllText(@"D:\Las\Sliced_4\Run 1S2224591_20220414-095942_0001\HeaderSlice.json"));

            Map map = new Map { MinX = sg.MinX, MaxX = sg.MaxX, MinY = sg.MinY, MaxY = sg.MaxY, MinZ = sg.MinZ, MaxZ = sg.MaxZ, ChunkSize = sg.Slices[0].MaxX - sg.Slices[0].MinX,
                                XScaleFactor = sg.XScaleFactor, YScaleFactor = sg.YScaleFactor, ZScaleFactor = sg.ZScaleFactor,
                                XOffset = sg.XOffset, YOffset = sg.YOffset, ZOffset = sg.ZOffset };

            Directory.CreateDirectory(Path.Combine(@"D:\Las\Sliced_4\Run 1S2224591_20220414-095942_0001", LeveledFolder));

            int i = 0;
            BlockingCollection<ChunkMeta> chunkMetas = new BlockingCollection<ChunkMeta>();
            //foreach (var s in sg.Slices.OrderByDescending(t => t.Count).Take(100))
            Parallel.ForEach(sg.Slices, s =>
            {
                string fileName = s.Path.Split('\\', '.')[1];
                var cm = Process(s, @"D:\Las\Sliced_4\Run 1S2224591_20220414-095942_0001", @"D:\Las\Sliced_4\Run 1S2224591_20220414-095942_0001", fileName + ".lasleveled");
                chunkMetas.Add(cm);
                //GC.Collect();
                Console.WriteLine(i++);
            });

            map.Chunks = chunkMetas.ToList();
            map.TotalChunks = map.Chunks.Count;
            map.TotalPoints = map.Chunks.Sum(t => t.Count);

            string jsonString = JsonSerializer.Serialize(map, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(@"D:\Las\Sliced_4\Run 1S2224591_20220414-095942_0001\LeveledSlices.json", jsonString);

            Console.WriteLine("Finish!");
        }

        static ChunkMeta Process(SliceMeta meta, string rootPath, string targetFolder, string fileName)
        {
            string relativePath = Path.Combine(LeveledFolder, fileName);
            string path = Path.Combine(targetFolder, relativePath);
            var d = GetLevelData(meta, rootPath);

            ChunkMeta chunkMeta = new ChunkMeta
            {
                Path = relativePath,
                MinX = meta.MinX,
                MinY = meta.MinY,
                MaxX = meta.MaxX,
                MaxY = meta.MaxY,
                Layers = d.OrderBy(t => t.Key).Select(t => t.Value.Count).ToArray()
            };
            chunkMeta.Count = chunkMeta.Layers.Sum();

            using (var stream = File.Open(path, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    writer.Write(chunkMeta.Count);
                    writer.Write(chunkMeta.Layers.Length);
                    int count = d.Count;
                    foreach(var l in d.OrderBy(t => t.Key))
                    {
                        writer.Write(l.Value.Count);
                    }
                    foreach (var points in d.OrderBy(t => t.Key).Select(t => t.Value))
                    {
                        foreach(var p in points)
                        {
                            writer.Write(p.X);
                            writer.Write(p.Y);
                            writer.Write(p.Z);
                            writer.Write(p.Grey);
                        }
                    }
                }
            }

            return chunkMeta;
        }

        static Dictionary<int, List<PointWithColor>> GetLevelData(SliceMeta meta, string rootPath)
        {
            List<PointWithColor> points = new List<PointWithColor>(meta.Count);
            using var binaryReader = new BinaryReader(File.OpenRead(Path.Combine(rootPath, meta.Path)));
            while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
            {
                PointWithColor point = new PointWithColor(binaryReader);
                points.Add(point);
            }

            var res = OctetTreeProducer.GetTrees(meta, points);
            Dictionary<int, List<PointWithColor>> dataByLevels = res[0].GetDataByLevels();
            for(int i = 1; i < res.Count; i++)
            {
                foreach(var p in res[i].GetDataByLevels())
                {
                    if(dataByLevels.TryGetValue(p.Key, out List<PointWithColor> data))
                    {
                        data.AddRange(p.Value);
                    }
                    else
                    {
                        dataByLevels.Add(p.Key, p.Value);
                    }
                }
            }

            return dataByLevels;
        }

        public class Map
        {
            public int TotalPoints { get; set; }
            public int TotalChunks { get; set; }

            public double XScaleFactor { get; set; }
            public double YScaleFactor { get; set; }
            public double ZScaleFactor { get; set; }

            public double XOffset { get; set; }
            public double YOffset { get; set; }
            public double ZOffset { get; set; }

            public double MaxX { get; set; }
            public double MaxY { get; set; }
            public double MaxZ { get; set; }

            public double MinX { get; set; }
            public double MinY { get; set; }
            public double MinZ { get; set; }

            public int ChunkSize { get; set; }
            public List<ChunkMeta> Chunks { get; set; }
        }
        public class ChunkMeta
        {
            public string Path { get; set; }
            public int Count { get; set; }
            public int MinX { get; set; }
            public int MaxX { get; set; }
            public int MinY { get; set; }
            public int MaxY { get; set; }
            public int[] Layers { get; set; }
        }
    }
}