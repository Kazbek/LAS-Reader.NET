using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LASReader.NET.Version_1_4
{
    public sealed class PointDataFormat7
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public ushort Intensity { get; set; }
        public byte ReturnNumber_NumberofReturns { get; set; }
        public byte ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine { get; set; }
        public byte Classification { get; set; }
        public byte UserData { get; set; }
        public short ScanAngle { get; set; }
        public ushort PointSourceID { get; set; }
        public double GPSTime { get; set; }
        public ushort Red { get; set; }
        public ushort Green { get; set; }
        public ushort Blue { get; set; }

        internal void ReadFrom(LasReader reader)
        {
            X = reader._binaryReader.ReadInt32() * reader.XScaleFactor + reader.XOffset;
            Y = reader._binaryReader.ReadInt32() * reader.YScaleFactor + reader.YOffset;
            Z = reader._binaryReader.ReadInt32() * reader.ZScaleFactor + reader.ZOffset;
            Intensity = reader._binaryReader.ReadUInt16();

            ReturnNumber_NumberofReturns = reader._binaryReader.ReadByte();
            ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine = reader._binaryReader.ReadByte();
            
            Classification = reader._binaryReader.ReadByte();
            UserData = reader._binaryReader.ReadByte();

            ScanAngle = reader._binaryReader.ReadInt16();
            PointSourceID = reader._binaryReader.ReadUInt16();

            GPSTime = reader._binaryReader.ReadDouble();

            Red = reader._binaryReader.ReadUInt16();
            Green = reader._binaryReader.ReadUInt16();
            Blue = reader._binaryReader.ReadUInt16();
        }
    }
}
