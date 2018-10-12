using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUIPanel : UIPanelBase {

    InputField userNmae;
    Button loginButton;
    private LoginRequest loginRequest;
    private bool isAllowLoadScene = false;

    protected override void Awake()
    {
        base.Awake();

        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        userNmae = transform.Find("AccountInputField").GetComponent<InputField>();
        loginButton.onClick.AddListener(OnLoginBtnClick);
        loginRequest = GetComponent<LoginRequest>();

    }

    private void OnLoginBtnClick()
    {
        if (string.Empty == userNmae.text.Trim()) return;
        string nickName = userNmae.text.Trim();
        loginRequest.SendRequest(nickName);
    }

    public void OnLoginCallback(ResponseCode code,string data)
    {
        print(code);
        if(code == ResponseCode.Success)
        {
            isAllowLoadScene = true;
            return;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (isAllowLoadScene)
        {
            SceneManager.LoadScene("ChatRoomScene");
        }
    }
}
