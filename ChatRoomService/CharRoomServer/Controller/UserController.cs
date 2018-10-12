using CharRoomServer.Model;
using ChatRoomServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharRoomServer.Controller
{
    public class UserController : BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }


        public string Login(string data,Client client,Server server)
        {
            string nickName = data;
            User user = new User(nickName);
            client.InitUser(user);
            server.OnClientLogin(client);
            return string.Format("{0};{1}", ((int)ResponseCode.Success).ToString(), data);
        }

        public string Logout(string data,Client client,Server server)
        {
            if(server.ChatRoomClients.Contains(client))
                server.ChatRoomClients.Remove(client);
            server.BroadcastChatRoom(ActionCode.OnlineUserList, "", client);
            return string.Format("{0};{1}", ((int)ResponseCode.Success).ToString(), data);
        }
    }
}
