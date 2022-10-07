using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace LASReader.NET.Version_1_4
{
    public sealed class LasReader
    {
        public char[] FileSignature { get; set; }
        public ushort FileSourceID { get; set; }
        public ushort GlobalEncoding { get; set; }

        public uint GuidData1 { get; }
        public ushort GuidData2 { get; }
        public ushort GuidData3 { get; }
        public char[] GuidData4 { get; }

        public byte VersionMajor { get; set; }
        public byte VersionMinor { get; set; }

        public char[] SystemIdentifier { get; set; }
        public char[] GeneratingSoftware { get; set; }

        public ushort FileCreationDayOfYear { get; }
        public ushort FileCreationYear { get; }
        public ushort HeaderSize { get; }
        public uint OffsetToPointData { get; }
        public uint NumberOfVariableLengthRecords { get; }

        public byte PointDataRecordFormat { get; set; }
        public ushort PointDataRecordLength { get; set; }

        public uint LegacyNumberOfPointRecords { get; set; }
        public BigInteger LegacyNumberOfPointByReturn { get; set; }

        public double XScaleFactor { get; }
        public double YScaleFactor { get; }
        public double ZScaleFactor { get; }

        public double XOffset { get; }
        public double YOffset { get; }
        public double ZOffset { get; }

        public double MaxX { get; }
        public double MaxY { get; }
        public double MaxZ { get; }

        public double MinX { get; }
        public double MinY { get; }
        public double MinZ { get; }

        public ulong StartOfWaveformDataPacketRecord { get; set; }
        public ulong StartOfFirstExtendedVariableLengthRecord { get; set; }
        public uint NumberOfExtendedVariableLengthRecords { get; set; }

        public ulong NumberOfPointRecords { get; set; }
        public BigInteger NumberOfPointByReturn { get; set; }


        public double SizeX => MaxX - MinX;
        public double SizeY => MaxY - MinY;
        public double SizeZ => MaxZ - MinZ;

        internal readonly BinaryReader _binaryReader;
        public LasReader(string lasFilePath)
        {
            _binaryReader = new BinaryReader(File.OpenRead(lasFilePath));
            FileSignature = _binaryReader.ReadChars(4);
            FileSourceID = _binaryReader.ReadUInt16();
            GlobalEncoding = _binaryReader.ReadUInt16();

            GuidData1 = _binaryReader.ReadUInt32();
            GuidData2 = _binaryReader.ReadUInt16();
            GuidData3 = _binaryReader.ReadUInt16();
            GuidData4 = _binaryReader.ReadChars(8);

            VersionMajor = _binaryReader.ReadByte();
            VersionMinor = _binaryReader.ReadByte();

            SystemIdentifier = _binaryReader.ReadChars(32);
            GeneratingSoftware = _binaryReader.ReadChars(32);

            FileCreationDayOfYear = _binaryReader.ReadUInt16();
            FileCreationYear = _binaryReader.ReadUInt16();
            HeaderSize = _binaryReader.ReadUInt16();
            OffsetToPointData = _binaryReader.ReadUInt32();
            NumberOfVariableLengthRecords = _binaryReader.ReadUInt32();

            PointDataRecordFormat = _binaryReader.ReadByte();
            PointDataRecordLength = _binaryReader.ReadUInt16();

            LegacyNumberOfPointRecords = _binaryReader.ReadUInt32();
            LegacyNumberOfPointByReturn = new BigInteger(_binaryReader.ReadBytes(20));

            XScaleFactor = _binaryReader.ReadDouble();
            YScaleFactor = _binaryReader.ReadDouble();
            ZScaleFactor = _binaryReader.ReadDouble();

            XOffset = _binaryReader.ReadDouble();
            YOffset = _binaryReader.ReadDouble();
            ZOffset = _binaryReader.ReadDouble();

            MaxX = _binaryReader.ReadDouble();
            MinX = _binaryReader.ReadDouble();

            MaxY = _binaryReader.ReadDouble();
            MinY = _binaryReader.ReadDouble();

            MaxZ = _binaryReader.ReadDouble();
            MinZ = _binaryReader.ReadDouble();

            StartOfWaveformDataPacketRecord = _binaryReader.ReadUInt64();
            StartOfFirstExtendedVariableLengthRecord = _binaryReader.ReadUInt64();
            NumberOfExtendedVariableLengthRecords = _binaryReader.ReadUInt32();

            NumberOfPointRecords = _binaryReader.ReadUInt64();
            NumberOfPointByReturn = new BigInteger(_binaryReader.ReadBytes(120));

            _binaryReader.BaseStream.Seek(OffsetToPointData, 0);
        }

        public void ReadTo(PointDataFormat7 point)
        {
            point.ReadFrom(this);
        }



        public void Dispose()
        {
            _binaryReader?.Dispose();
        }
    }
}
