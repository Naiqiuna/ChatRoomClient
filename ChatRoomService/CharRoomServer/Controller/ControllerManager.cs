using ChatRoomServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CharRoomServer.Controller
{
    public class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDic = new Dictionary<RequestCode, BaseController>();
        private Server server;
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }

        private void InitController()
        {
            controllerDic.Add(RequestCode.User, new UserController());
            controllerDic.Add(RequestCode.ChatRoom, new ChatRoomController());
        }



        public void RequestHandle(RequestCode requestCode,ActionCode actionCode,string data,Client client,Action<ActionCode,string,Client> responseCallback)
        {
            Console.WriteLine("控制管理器开始解析请求命令");
            BaseController baseController;
            controllerDic.TryGetValue(requestCode, out baseController);
            if(baseController == null)
            {
                Console.WriteLine("无法获取处理[" + requestCode + "] 的 类");
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            Console.WriteLine("解析方法名为" + methodName);
            MethodInfo mi = baseController.GetType().GetMethod(methodName);

            object[] parameters = new object[] { data, client, server };
            object o = mi.Invoke(baseController, parameters);
            if (o.ToString() == string.Empty) return;
            responseCallback(actionCode,o as string,client);
        }



    }
}
