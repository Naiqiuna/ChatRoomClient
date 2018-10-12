using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatRoomButton : MonoBehaviour {

    private ChatRoomPanel panel;

    public void InitPanel(ChatRoomPanel _panel)
    {
        this.panel = _panel;
    }

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnChatRoomBtnClick);

    }

    private void OnChatRoomBtnClick()
    {

        panel.OnChatRoomBtnClick(gameObject.name.Replace("Button", ""), button);
    }
}
