using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ClientManager : BaseManager{

    private IPEndPoint serverEndPoint;
    private IPEndPoint clientEndPoint;
    private Socket clientSocket;
    private Message message;


    public ClientManager(GameFacade facade):base(facade)
    {
        this.facade = facade;
        serverEndPoint = new IPEndPoint(IPAddress.Parse(DataManager.SERVER_IP),DataManager.SERVER_PORT);
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        message = new Message();
    }

    public void Start()
    {
        try
        {
            clientSocket.Connect(serverEndPoint);
            clientEndPoint = clientSocket.LocalEndPoint as IPEndPoint;
            clientSocket.BeginReceive(message.Buffer, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            int length = clientSocket.EndReceive(ar);
            Debug.Log("收到来自服务器的数据大小为" + length);
            if(length > 0)
            {
                message.UnpackData(length, ResponseCallback);
                clientSocket.BeginReceive(message.Buffer, message.StartIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Close();
            return;
        }
    }

   
    


    public void Send(RequestCode requestCode,ActionCode actionCode,string data)
    {
        ChecekAndReconnect();
        Debug.Log("向服务器发送消息：" + requestCode + actionCode + data);
        byte[] messageBytes = Message.PackData(requestCode, actionCode, data);
        Debug.Log(messageBytes.Length);
        clientSocket.Send(messageBytes);
    }

    private void ChecekAndReconnect()
    {
        if (clientSocket.Connected) return;
        Start();
    }


    private void ResponseCallback(ActionCode actionCode,ResponseCode responseCode, string data)
    {
        facade.ResponseHandle(actionCode,responseCode,data);
    }

    public void Close()
    {
        clientSocket.Close();
        Debug.Log("关闭客户端");
    }


}
