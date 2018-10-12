using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LoginRequest : BaseRequest {

    private LoginUIPanel loginUIManager;

    

    protected override void Awake()
    {
        actionCode = ActionCode.Login;
        requestCode = RequestCode.User;
        loginUIManager = GetComponent<LoginUIPanel>();
        base.Awake();
    }

    public override void OnResponse(ResponseCode code,string data)
    {
        loginUIManager.OnLoginCallback(code,data);
    }
}
