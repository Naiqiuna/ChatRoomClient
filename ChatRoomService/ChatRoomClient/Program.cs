using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoomClient
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                Client client = new Client();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55555));
            }
            Console.ReadKey(true);
        }
    }
}
