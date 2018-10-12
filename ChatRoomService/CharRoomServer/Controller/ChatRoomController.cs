using CharRoomServer.Model;
using ChatRoomServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharRoomServer.Controller
{
    public class ChatRoomController : BaseController
    {
        public ChatRoomController()
        {
            requestCode = RequestCode.ChatRoom;
        }


        public string OnlineUserList(string data,Client client,Server server)
        {
            string response = ((int)ResponseCode.Success).ToString() + ";";
            for (int i = 0; i < server.ChatRoomClients.Count; i++)
            {
               User user = server.ChatRoomClients[i].User;
                response += user.NickName + (server.ChatRoomClients.Count - 1 == i ? "" : ";");
            }
            return response;
        }

        public string MessageList(string data,Client client,Server server)
        {
            string response = ((int)ResponseCode.Success).ToString() + ";";
            response += string.Format("{0}:{1}", client.User.NickName, data);
            server.BroadcastChatRoom(ActionCode.MessageList, response, client);
            return response;
        }
    }
}
