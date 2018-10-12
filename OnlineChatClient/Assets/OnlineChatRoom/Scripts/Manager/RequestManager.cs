using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : BaseManager {

    public RequestManager(GameFacade facade) : base(facade) { }

    private Dictionary<ActionCode, BaseRequest> dic = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode code,BaseRequest request)
    {
        if (dic.ContainsKey(code)) return;
        dic.Add(code, request);
    }

    public void RemoveRequest(ActionCode code)
    {
        if (!dic.ContainsKey(code)) return;
        dic.Remove(code);
    }

    public void ResponseHandle(ActionCode actionCode,ResponseCode responseCode,string data)
    {
        Debug.Log(actionCode);
        BaseRequest request;
        dic.TryGetValue(actionCode,out request);
        if (request == null)
        {
            Debug.Log("无法找到" + actionCode + "对应的请求类");
            return;
        }
        request.OnResponse(responseCode, data);
    }
}