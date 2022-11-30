using LASReader.NET.Version_1_4;
using SimpleRead.Slicers;
using System.Text.Json;

namespace SimpleRead
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //200 000 int is 20meter
            Console.WriteLine("Hello, World!");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 1S2224591_20220414-095942_0001.las");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 1S2225630_20220414-095942_0001.las");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 2_1S2224591_20220414-105427_0002.las");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 2_1S2225630_20220414-105427_0002.las");
            //LasReader reader = new LasReader(@"D:\Las\lasers\Unnamed Run  0 S2224591_20201123-110748_0000.las");
            LasReader reader = new LasReader(@"D:\Las\Run 1S2224591_20220414-095942_0001.las");

            string targetPath = @"D:\Las\Sliced_4\Run 1S2224591_20220414-095942_0001\";
            string targetSubFolder = "Chunks";
            Directory.CreateDirectory(Path.Combine(targetPath, targetSubFolder));

            PointDataFormat7Light point = new PointDataFormat7Light();
            int cupSize = 40_000;

            Dictionary<int, Dictionary<int, WritePair>> slicers = new Dictionary<int, Dictionary<int, WritePair>>();
            List<SliceMeta> sliceMetas = new List<SliceMeta>(100_000);

            SliceGeneral sliceGeneral = new SliceGeneral
            {
                XScaleFactor = reader.XScaleFactor,
                YScaleFactor = reader.YScaleFactor,
                ZScaleFactor = reader.ZScaleFactor,

                XOffset = reader.XOffset,
                YOffset = reader.YOffset,
                ZOffset = reader.ZOffset,

                MaxX = reader.MaxX,
                MaxY = reader.MaxY,
                MaxZ = reader.MaxZ,

                MinX = reader.MinX,
                MinY = reader.MinY,
                MinZ = reader.MinZ
            };


            for (ulong i = 0; i < reader.NumberOfPointRecords; i++)
            {
                reader.ReadTo(point);
                sliceGeneral.TotalPoints++;

                int cupX = LowerBound(cupSize, point.X);//point.X / cupSize;
                int cupY = LowerBound(cupSize, point.Y);//point.Y / cupSize;

                if (!slicers.TryGetValue(cupX, out Dictionary<int, WritePair> slicersY))
                {
                    slicersY = new Dictionary<int, WritePair>();
                    slicers.Add(cupX, slicersY);
                }

                if (!slicersY.TryGetValue(cupY, out WritePair slicePair))
                {
                    SliceMeta sm = new SliceMeta
                    {
                        Path = Path.Combine(targetSubFolder, $"SLICE_X_{cupX * cupSize}_{(cupX + 1) * cupSize}_Y_{cupY * cupSize}_{(cupY + 1) * cupSize}.lasslice"),
                        MinX = cupX * cupSize,
                        MaxX = (cupX + 1) * cupSize,
                        MinY = cupY * cupSize,
                        MaxY = (cupY + 1) * cupSize,

                        FactMinX = point.X,
                        FactMaxX = point.X,
                        FactMinY = point.Y,
                        FactMaxY = point.Y,
                        FactMinZ = point.Z,
                        FactMaxZ = point.Z
                    };
                    sliceMetas.Add(sm);
                    slicePair = new WritePair { SliceWriter = new SliceWriter(Path.Combine(targetPath, sm.Path), 500) , SliceMeta = sm };
                    slicersY.Add(cupY, slicePair);
                }

                slicePair.SliceWriter.Write(point);
                slicePair.SliceMeta.Count++;

                if(slicePair.SliceMeta.FactMinX > point.X) slicePair.SliceMeta.FactMinX = point.X;
                if(slicePair.SliceMeta.FactMinY > point.Y) slicePair.SliceMeta.FactMinY = point.Y;
                if(slicePair.SliceMeta.FactMinZ > point.Z) slicePair.SliceMeta.FactMinZ = point.Z;

                if (slicePair.SliceMeta.FactMaxX < point.X) slicePair.SliceMeta.FactMaxX = point.X;
                if (slicePair.SliceMeta.FactMaxY < point.Y) slicePair.SliceMeta.FactMaxY = point.Y;
                if (slicePair.SliceMeta.FactMaxZ < point.Z) slicePair.SliceMeta.FactMaxZ = point.Z;

                if (i % 100_000 == 0)
                {
                    Console.WriteLine($"Processed: {i} of {reader.NumberOfPointRecords} ({(100d * i / reader.NumberOfPointRecords)}). Slices: {sliceMetas.Count}");
                }

                //if (point.X != 0 || point.Y != 0 || point.Z != 0)
                //{
                //    var z = 1;
                //}
            }

            Console.WriteLine($"Start closing");

            foreach (var d in slicers)
            {
                foreach (var s in d.Value)
                {
                    s.Value.SliceWriter.Close();
                }
            }

            Console.WriteLine($"{sliceMetas.Count} slices!");

            sliceGeneral.Slices = sliceMetas;
            sliceGeneral.TotalSlices = sliceMetas.Count;
            string jsonString = JsonSerializer.Serialize(sliceGeneral, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(targetPath + "HeaderSlice.json", jsonString);

            Console.WriteLine($"Finished! {sliceMetas.Count} slices!");

        }

        public static int LowerBound(int groupSize, int value)
        {
            int lowerBound = value / groupSize;
            int remainder = value % groupSize;
            if (remainder < 0)
                lowerBound--;

            return lowerBound;
        }
    }

    public class WritePair
    {
        public SliceWriter SliceWriter { get; set; }
        public SliceMeta SliceMeta { get; set; }
    }

    public class SliceGeneral
    {
        public int TotalPoints { get; set; }
        public int TotalSlices { get; set; }

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

        public List<SliceMeta> Slices { get; set; }
    }
}