using CharRoomServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatRoomServer.Servers
{
    public class Client
    {
        private Socket clientSocket;
        public Socket ClientSocket { get { return clientSocket; } }

        private IPEndPoint ipEndPoint;
        public IPEndPoint ClientIPEndPoint { get { return ipEndPoint; } }
        Message message;
        private Server server;
        private User user;
        public User User { get { return user; } }
        private int timeoutCount = 0;
        public Client(Socket socket, Server server)
        {
            this.clientSocket = socket;
            this.server = server;
            ipEndPoint = socket.RemoteEndPoint as IPEndPoint;
            Console.WriteLine("[" + ipEndPoint + "]client connected server ...");
            message = new Message();
        }

        public void Start()
        {
            clientSocket.BeginReceive(message.Buffer, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveMessage, null);
        }

        public void Send(ActionCode actionCode, string data)
        {
            byte[] bytes = Message.PackData(actionCode, data);
            clientSocket.Send(bytes);
            Console.WriteLine("发送成功数据");
        }

        private void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                int length = clientSocket.EndReceive(ar);
                Console.WriteLine("收到[" + ipEndPoint + "]客户端发送" + length + "字节");
                if (length > 0)
                {
                    message.UnpackData(length, OnResponse);
                    clientSocket.BeginReceive(message.Buffer, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveMessage, null);
                }
                else
                {
                    timeoutCount++;
                    if(timeoutCount >= 1)
                    {
                        server.OnClientLeave(this);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Close();
            }


        }

        private void OnResponse(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.ResponseHandle(requestCode, actionCode, data, this);
        }


        public void Close()
        {
            clientSocket.Close();
            Console.WriteLine("关闭与[" + ipEndPoint + "]客户端的连接");
        }

        public void InitUser(User user)
        {
            this.user = user;
        }
    }


}
