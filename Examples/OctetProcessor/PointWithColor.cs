namespace OctetProcessor
{
    public class PointWithColor
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Grey { get; set; }

        public double CalculateX(double xScaleFactor, double xOffset) => X * xScaleFactor + xOffset;
        public double CalculateY(double yScaleFactor, double yOffset) => Y * yScaleFactor + yOffset;
        public double CalculateZ(double zScaleFactor, double zOffset) => X * zScaleFactor + zOffset;

        public PointWithColor() { }
        public PointWithColor(BinaryReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Z = reader.ReadInt32();
            Grey = reader.ReadByte();
        }

        public int SqrDistance(PointWithColor target)
        {
            return (X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y) + (Z - target.Z) * (Z - target.Z);
        }

        public void ReadFrom(BinaryReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            Z = reader.ReadInt32();
            Grey = reader.ReadByte();
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(Grey);
        }
    }
}
