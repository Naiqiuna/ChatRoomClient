using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharRoomServer.Model
{
    public class User
    {
        private string nickName;
        public string NickName { get { return nickName; } }
        public User(string name)
        {
            this.nickName = name;
        }
    }
}
