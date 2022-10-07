using LASReader.NET.Version_1_4;

namespace SimpleRead
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            LasReader reader = new LasReader(@"D:\Las\Run 1S2224591_20220414-095942_0001.las");

            PointDataFormat7 point = new PointDataFormat7();

            for (ulong i = 0; i < 100000/*reader.NumberOfPointRecords*/; i++)
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