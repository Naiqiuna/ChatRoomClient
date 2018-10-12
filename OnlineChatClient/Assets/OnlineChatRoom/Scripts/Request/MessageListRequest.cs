using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageListRequest : BaseRequest {

    private ChatRoomPanel panel;

    protected override void Awake()
    {
        actionCode = ActionCode.MessageList;
        requestCode = RequestCode.ChatRoom;
        base.Awake();
        panel = GetComponent<ChatRoomPanel>();
    }

    public override void OnResponse(ResponseCode code, string data)
    {
        panel.OnMessageListResponse(code, data);
    }

    public override void SendRequest(string data)
    {
        gameFacade.SendRequest(requestCode, actionCode, data);
    }
}
