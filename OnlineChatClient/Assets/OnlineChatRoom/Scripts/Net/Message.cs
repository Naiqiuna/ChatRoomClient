using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Message{

    private byte[] buffer = new byte[1024];
    public byte[] Buffer { get { return buffer; } }
    private int startIndex = 0;
    public int StartIndex { get { return startIndex; } }
    public int RemainSize { get { return buffer.Length - startIndex; } }


    public static byte[] PackData(RequestCode requestCode,ActionCode actionCode,string data)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] requestBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionBytes = BitConverter.GetBytes((int)actionCode);
        byte[] bodyBytes = requestBytes.Concat(actionBytes).Concat(dataBytes).ToArray();
        int bodyLength = bodyBytes.Length;
        byte[] lengthBytes = BitConverter.GetBytes(bodyLength);
        byte[] newBytes = lengthBytes.Concat(bodyBytes).ToArray();
        return newBytes;
    }


    public void UnpackData(int dataAmount,Action<ActionCode,ResponseCode,string> responseCallback)
    {
        int remainCount = dataAmount + startIndex;
        while (true)
        {
            if (remainCount <= 4) break;
            int dataLength = BitConverter.ToInt32(buffer, 0);
            if (dataLength >= remainCount - 4)
            {
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(buffer, 4);
                ResponseCode responseCode = (ResponseCode)BitConverter.ToInt32(buffer, 8);
                string data = Encoding.UTF8.GetString(buffer, 12, dataLength - 8);
               Debug.Log("解析数据为：" + actionCode + responseCode + data);
                responseCallback(actionCode, responseCode, data);
                Array.Copy(buffer, dataLength + 4, buffer, 0, remainCount - dataLength - 4);
                remainCount -= (dataLength + 4);
            }
            else break;
        }
        startIndex = remainCount;
    }
}
