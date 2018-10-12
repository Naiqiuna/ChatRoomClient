using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour {

    private static GameFacade instance;
    public static GameFacade Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<GameFacade>();
            }
                
            return instance;
        }
    }
    private ClientManager clientManager;
    private RequestManager requestManager;
    private UserInfo m_userInfo;
    public UserInfo userInfo { get { return m_userInfo; } }
    private UIManager uiManager;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitManager();
    }

    private void Start()
    {
      
        StartManager();
    }

    private void InitManager()
    {
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
    }

    public void Login(UserInfo user)
    {
        m_userInfo = user;
    }

    private void StartManager()
    {
        clientManager.Start();
    }

    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
        clientManager.Send(requestCode, actionCode, data);
    }
    

    public void ResponseHandle(ActionCode actionCode,ResponseCode responseCode,string data)
    {
        requestManager.ResponseHandle(actionCode, responseCode, data);
    }

    public void RemoveRequest(ActionCode code)
    {
        requestManager.RemoveRequest(code);
    }

    public void AddRequest(ActionCode code,BaseRequest request)
    {
        requestManager.AddRequest(code, request);
    }

    private void OnDestroy()
    {
        clientManager.Close();
    }


}
