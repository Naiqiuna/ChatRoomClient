using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using CharRoomServer.Controller;

namespace ChatRoomServer.Servers
{
    public class Server
    {
        private string ip;
        public string IP { get { return ip; } }
        private int port;
        public int Port { get { return port; } }

        private IPEndPoint ipEndPoint;
        public IPEndPoint IPEndPoint { get { return ipEndPoint; } }
        private Socket serverSocket;
        private List<Client> connectingClients;
        public List<Client> ConnectingClients { get { return connectingClients; } }
        private List<Client> chatRoomClients;
        public List<Client> ChatRoomClients { get { return chatRoomClients; } }
        private ControllerManager controllerManager;
        public Server(string _ip, int _port)
        {
            this.ip = _ip;
            this.port = _port;
            connectingClients = new List<Client>();
            chatRoomClients = new List<Client>();
            ipEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            controllerManager = new ControllerManager(this);
           
        }

        public void Start()
        {
            try
            {
                serverSocket.Bind(ipEndPoint);
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptClient, null);
                Console.WriteLine("[" + ipEndPoint + "]Server start listening ...");
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
          
        }

        private void AcceptClient(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            connectingClients.Add(client);
            serverSocket.BeginAccept(AcceptClient, null);
        }

       
        public void ResponseHandle(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            controllerManager.RequestHandle(requestCode, actionCode, data, client,SendResponse);
        }


        public void SendResponse(ActionCode code,string data,Client client)
        {
            client.Send(code, data);
        }
        
        public void RemoveClient(Client client)
        {
            if (connectingClients.Contains(client))
            {
                connectingClients.Remove(client);
            }
               
        }

        public void Broadcast(ActionCode code,string data,Client client)
        {
            for (int i = 0; i < connectingClients.Count; i++)
            {
                connectingClients[i].Send(code, data);
            }
        }

        public void BroadcastChatRoom(ActionCode code,string data,Client client)
        {
            for (int i = 0; i < chatRoomClients.Count; i++)
            {
                if (chatRoomClients[i] == client) continue;
                chatRoomClients[i].Send(code, data);
            }
        }


        public void OnClientLeave(Client client)
        {
            client.Close();
            RemoveClient(client);
            if(chatRoomClients.Contains(client))
                chatRoomClients.Remove(client);
            controllerManager.RequestHandle(RequestCode.ChatRoom, ActionCode.OnlineUserList, "", client,Broadcast);
        }

        public void OnClientLogin(Client client)
        {
            if (!chatRoomClients.Contains(client))
                chatRoomClients.Add(client);
            controllerManager.RequestHandle(RequestCode.ChatRoom, ActionCode.OnlineUserList, "", client, BroadcastChatRoom);
        }
    }

     
}
