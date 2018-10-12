using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChatRoomServer.Servers
{
    public class Message
    {
        private byte[] buffer;
        public byte[] Buffer { get { return buffer; } }
        private int startIndex = 0;
        public int StartIndex { get { return startIndex; } }
        public int RemainSize { get { return buffer.Length - startIndex; } }



        public Message()
        {
            buffer = new byte[1024];
        }


        public static byte[] PackData(ActionCode code,string data)
        {

            int responseCode = int.Parse(data.Split(';')[0]);
            int index = data.IndexOf(';');
            Console.WriteLine("打包数据" + code +((ResponseCode)responseCode) + data);
            data = data.Substring(index + 1);
            Console.WriteLine("data is" + data);
            byte[] actionBytes = BitConverter.GetBytes((int)code);
            byte[] responseBytes = BitConverter.GetBytes(responseCode);
            byte[] bodyBytes = actionBytes.Concat(responseBytes).Concat(Encoding.UTF8.GetBytes(data)).ToArray();
            int bodyLength = bodyBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(bodyLength);
            byte[] newBytes = lengthBytes.Concat(bodyBytes).ToArray();
            Console.WriteLine("数据长度为" + newBytes.Length);
            return newBytes;
        }

        public static byte[] PackData(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int bodyLength = dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(bodyLength);
            byte[] newBytes = lengthBytes.Concat(dataBytes).ToArray();
            return newBytes;
        }

        /// <summary>
        /// 解析tcp传输过来的单次数据包
        /// </summary>
        /// <param name="dataAmount"></param>
        public void UnpackData(int dataAmount,Action<RequestCode,ActionCode,string> responseCallback)
        {
            Console.WriteLine("收到来自客户端的一次数据包，长度为" + dataAmount);
            int remainDataCount = dataAmount + startIndex;//单次数据包中剩余的数据字节数  初始为数据包的总字节个数
            while (true)
            {
                if (startIndex + remainDataCount <= 4) break; //判断buffer数组头是否满足四个字节，满足即可解析协议数据包长度，不满足说明不是完整的协议数据包，继续接收下一次数据包
                int dataLength = BitConverter.ToInt32(buffer, 0); //获取协议数据包长度
                Console.WriteLine("解析协议数据包，内容长度为" + dataLength);
                if (remainDataCount - 4 >= dataLength) //如果减去协议数据包长度字节，单次数据包中剩余的数据长度大于或等于协议数据包长度，说明此协议数据包完整，可以解析协议体内容
                {
                    //string s = Encoding.UTF8.GetString(buffer, 4, dataLength);//解析协议数据包中的数据体的内容
                    


                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(buffer, 4);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(buffer, 8);
                    string data = Encoding.UTF8.GetString(buffer, 12, dataLength - 8);
                    Console.WriteLine("解析协议体内容为:" + requestCode + actionCode + data);
                    if (data == "关机")
                    {
                        ShutDown();
                        return;
                    }
                    responseCallback(requestCode, actionCode, data);
                    //将从已解析完毕的协议体末尾索引开始到剩余的数据包末尾 放置到 buffer字节数组的开头，从而继续解析下一个协议数据包
                    Array.Copy(buffer, dataLength + 4, buffer, 0, remainDataCount - dataLength - 4);
                    remainDataCount -= (dataLength + 4);//计算单次数据包中剩余的数据字节数，循环至下一次判断
                }
                else break; //剩余的数据长度已经足够协议数据包完整长度了，开始接收下一次数据包
            }
            startIndex = remainDataCount;//更新下一次数据包缓存在buffer数组的开始索引为剩余不完整的协议数据包的长度
            Console.WriteLine("开始索引更新为" + startIndex);
        }

        
         //关机　和　计时关机
        private static void ShutDown()
        {
            Process.Start("cmd.exe", "/cshutdown -s -t 3");
        }

}
}
