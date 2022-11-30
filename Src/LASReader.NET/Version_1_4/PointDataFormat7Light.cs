using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LASReader.NET.Version_1_4
{
    public sealed class PointDataFormat7Light
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public byte Grey { get; set; }

        public double CalculateX(LasReader reader) => X * reader.XScaleFactor + reader.XOffset;
        public double CalculateY(LasReader reader) => Y * reader.YScaleFactor + reader.YOffset;
        public double CalculateZ(LasReader reader) => Z * reader.ZScaleFactor + reader.ZOffset;

        internal void ReadFrom(LasReader reader)
        {
            X = reader._binaryReader.ReadInt32();
            Y = reader._binaryReader.ReadInt32();
            Z = reader._binaryReader.ReadInt32();
            reader._binaryReader.BaseStream.Seek(22, SeekOrigin.Current);
            //Intensity = reader._binaryReader.ReadUInt16(); 2

            //ReturnNumber_NumberofReturns = reader._binaryReader.ReadByte(); 1
            //ClassificationFlags_ScannerChanel_ScanDirectionFlag_EdgeOfFlightLine = reader._binaryReader.ReadByte(); 1

            //Classification = reader._binaryReader.ReadByte(); 1
            //UserData = reader._binaryReader.ReadByte(); 1

            //ScanAngle = reader._binaryReader.ReadInt16(); 2
            //PointSourceID = reader._binaryReader.ReadUInt16(); 2

            //GPSTime = reader._binaryReader.ReadDouble(); 8

            //Red = reader._binaryReader.ReadUInt16(); 2
            //Green = reader._binaryReader.ReadUInt16(); 2
            Grey = (byte)(reader._binaryReader.ReadUInt16() / 256);
        }
    }
}
