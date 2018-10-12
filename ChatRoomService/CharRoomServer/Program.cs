using ChatRoomServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatRoomServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("172.18.103.139", 55555);
            server.Start();
            Console.ReadKey(true);
        }
    }
}
