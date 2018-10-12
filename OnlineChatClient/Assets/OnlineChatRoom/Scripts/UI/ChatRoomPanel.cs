using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChatRoomPanel : MonoBehaviour {

    private ChatRoomButton[] buttons;
    private LogoutRequest logoutRequest;
    private OnlineUserListRequest userListRequest;
    private MessageListRequest messageListRequest;
    private bool mLogoutSuccess = false;
    [SerializeField] private RectTransform userBarContent;
    [SerializeField] private RectTransform messageBarContent;
    [SerializeField] private InputField messageInput;
    private float spacing = 10;
    private string[] names;
    private bool onlineUserListSuccess = false;
    private bool messageListSuccess = false;
    private string receivedMessage = string.Empty;
    private List<GameObject> onlineUserObjs = new List<GameObject>();
    private GameObject messageTextPrefab;
    private void Awake()
    {
        buttons = GetComponentsInChildren<ChatRoomButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].InitPanel(this);
        }
        logoutRequest = GetComponent<LogoutRequest>();
        userListRequest = GetComponent<OnlineUserListRequest>();
        messageListRequest = GetComponent<MessageListRequest>();
        messageTextPrefab = Resources.Load<GameObject>("UI/ChatRoomPanel/MessageText");
        messageBarContent.sizeDelta = Vector2.zero;
    }


    private void Start()
    {
        RefreshOnlineUserList();
    }

    void RefreshOnlineUserList()
    {
        userListRequest.SendRequest("null");
    }

    public void OnChatRoomBtnClick(string type,Button button)
    {
        print(type);
        switch (type)
        {
            case "Logout":
                logoutRequest.SendRequest("请求登出");
                break;
            case "Send":
                if(messageInput.text != string.Empty)
                {
                    messageListRequest.SendRequest(messageInput.text);
                }
                break;
            default:
                break;
        }
    }


    public void OnLogoutResponse(ResponseCode code,string data)
    {
        if(code == ResponseCode.Success)
        {
            mLogoutSuccess = true;
        }
    }


    public void OnOnlineUserListResponse(ResponseCode code,string data)
    {
        if(code == ResponseCode.Success)
        {
            names = data.Split(';');
            for (int i = 0; i < names.Length; i++)
            {
                print(names[i]);
            }
            onlineUserListSuccess = true;
        }
    }

    public void OnMessageListResponse(ResponseCode code,string data)
    {
        if (code == ResponseCode.Success)
        {
            receivedMessage = data;
            messageListSuccess = true;
        }
    }




    private void UpdateOnlineUserList()
    {
        if(onlineUserObjs.Count != 0)
        {
            for (int i = 0; i < onlineUserObjs.Count; i++)
            {
                Destroy(onlineUserObjs[i]);
            }
            onlineUserObjs.Clear();
        }
        GameObject textPrefab = Resources.Load<GameObject>("UI/ChatRoomPanel/UserText");
        Vector2 newSize = Vector2.zero;
        for (int i = 0; i < names.Length; i++)
        {
            GameObject newObj = Instantiate(textPrefab, userBarContent);
            newObj.GetComponent<Text>().text = names[i];
            onlineUserObjs.Add(newObj);
            print(newObj.GetComponent<Text>().text);
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            if (i == 0)
            {
                rectTransform.anchoredPosition = new Vector2(0, -newSize.y - rectTransform.sizeDelta.y / 2);
                newSize.y += rectTransform.sizeDelta.y;
            }
            else
            {
                rectTransform.anchoredPosition = new Vector2(0, -newSize.y - rectTransform.sizeDelta.y / 2 - spacing);
                newSize.y += rectTransform.sizeDelta.y + spacing;
            }
        }
        userBarContent.sizeDelta = newSize;
    }

   
    private void UpdateMessageBarList()
    {
        Vector2 newSize = messageBarContent.sizeDelta;
        GameObject newObj = Instantiate(messageTextPrefab, messageBarContent);
        newObj.GetComponent<Text>().text = receivedMessage;
        RectTransform rectTransform = newObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -newSize.y - rectTransform.sizeDelta.y / 2 - spacing);
        newSize.y += rectTransform.sizeDelta.y + spacing;
        messageBarContent.sizeDelta = newSize;
    }

    private void Update()
    {
        if (mLogoutSuccess)
        {
            SceneManager.LoadScene("LoginScene");
        }
        if (onlineUserListSuccess)
        {
            UpdateOnlineUserList();
            onlineUserListSuccess = false;
        }
        if (messageListSuccess)
        {
            UpdateMessageBarList();
            messageListSuccess = false;
        }

    }
}
