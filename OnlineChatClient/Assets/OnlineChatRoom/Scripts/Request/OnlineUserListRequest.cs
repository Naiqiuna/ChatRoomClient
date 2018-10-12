using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineUserListRequest : BaseRequest {

    private ChatRoomPanel panel;

    protected override void Awake()
    {
        actionCode = ActionCode.OnlineUserList;
        requestCode = RequestCode.ChatRoom;
        base.Awake();

        panel = GetComponent<ChatRoomPanel>();
    }

    public override void OnResponse(ResponseCode code, string data)
    {
        panel.OnOnlineUserListResponse(code,data); 
    }
}
