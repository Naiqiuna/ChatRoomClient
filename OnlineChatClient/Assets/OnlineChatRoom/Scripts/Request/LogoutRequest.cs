using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LogoutRequest : BaseRequest {

    private ChatRoomPanel panel;

    protected override void Awake()
    {
        actionCode = ActionCode.Logout;
        requestCode = RequestCode.User;

        panel = GetComponent<ChatRoomPanel>();
    }





    public override void OnResponse(ResponseCode code,string data)
    {
        panel.OnLogoutResponse(code,data);
    }
}
