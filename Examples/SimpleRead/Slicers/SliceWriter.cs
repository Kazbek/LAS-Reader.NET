using LASReader.NET.Version_1_4;
using System.Text;

namespace SimpleRead.Slicers
{
    public class SliceWriter : IDisposable
    {
        private BinaryWriter _binaryWriter;
        private int _flushLimit;
        private int _flushLimitCounter = 0;
        public SliceWriter(string filePath, int flushLimit)
        {
            _flushLimit = flushLimit;
            _binaryWriter = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write), Encoding.ASCII);

        }

        public void Write(PointDataFormat7Light point)
        {
            _binaryWriter.Write(point.X);
            _binaryWriter.Write(point.Y);
            _binaryWriter.Write(point.Z);
            _binaryWriter.Write(point.Grey);

            if (_flushLimitCounter++ > _flushLimit)
            {
                _flushLimitCounter = 0;
                _binaryWriter.Flush();
            }
        }

        public void Close()
        {
            FileStream writeStream = (FileStream)_binaryWriter.BaseStream;
            writeStream.Close();
            _binaryWriter.Close();
        }

        public void Dispose()
        {
            this.Close();
            this._binaryWriter?.Dispose();
        }
    }
}
