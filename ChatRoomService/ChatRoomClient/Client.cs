using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomClient
{
    public class Client
    {
        private Socket clientSocket;
        private IPEndPoint ipEndPoint;

        public Client()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("创建客户端成功");
        }

        public void Connect(IPEndPoint serverEndPoint)
        {
            clientSocket.Connect(serverEndPoint);
            ipEndPoint = clientSocket.LocalEndPoint as IPEndPoint;
            Console.WriteLine("[" + ipEndPoint + "]客户端连接服务器[" + serverEndPoint + "]成功");
        }


        public void Send(string message)
        {
            clientSocket.Send(Message.PackData(message));
        }



        public void Disconnect()
        {
            clientSocket.Close();
        }
    }
}
