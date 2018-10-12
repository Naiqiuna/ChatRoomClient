using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomClient
{
    public class Message
    {
        private byte[] buffer;
        public byte[] Buffer { get { return buffer; } set { buffer = value; } }
        private int startIndex;
        public int StartIndex { get { return startIndex; } set { startIndex = value; } }
        public int RemainSize { get { return buffer.Length - startIndex; } }

        public static byte[] PackData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataLength = dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(dataLength);
            byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
            return newBytes;
        }
        
    }
}
