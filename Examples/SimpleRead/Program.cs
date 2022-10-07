using LASReader.NET.Version_1_4;

namespace SimpleRead
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 1S2224591_20220414-095942_0001.las");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 1S2225630_20220414-095942_0001.las");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 2_1S2224591_20220414-105427_0002.las");
            //LasReader reader = new LasReader(@"F:\Downloads\А-104_КМ82_КМ121_М_Дмитровское шоссе\LAS\lasers\Run 2_1S2225630_20220414-105427_0002.las");
            //LasReader reader = new LasReader(@"D:\Las\lasers\Unnamed Run  0 S2224591_20201123-110748_0000.las");
            LasReader reader = new LasReader(@"D:\Las\Run 1S2224591_20220414-095942_0001.las");

            PointDataFormat7 point = new PointDataFormat7();

            for (ulong i = 0; i < 10000/*reader.NumberOfPointRecords*/; i++)
            {
                reader.ReadTo(point);
                if(point.X != 0 || point.Y != 0 || point.Z != 0)
                {
                    var z = 1;
                }
            }
        }
    }
}