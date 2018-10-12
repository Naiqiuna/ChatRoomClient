using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PanelType
{
    None,
    Main,
    Window
}

public class UIPanelBase : MonoBehaviour {

    protected int uid = -1;
    public int UID { get { return uid; } set { uid = value; } }
    protected PanelType type = PanelType.Main;
    public PanelType Type { get { return type; } }
    protected RectTransform rectTransform;
    protected RectTransform back;


	protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (transform.Find("Back") != null)
        {
            back = transform.Find("Back").GetComponent<RectTransform>();
            Image image = back.GetComponent<Image>();
            if (image.sprite != null)
            {
                back.sizeDelta = new Vector2(Screen.width, image.mainTexture.height / (image.mainTexture.width / Screen.width));
            }
            else
                back.sizeDelta = new Vector2(Screen.width, Screen.height);
            back.anchoredPosition = Vector2.zero;
        }
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

       
    }



    protected virtual void Start()
    {

    }


    protected virtual void Update()
    {

    }


    public virtual void Enter()
    {
        rectTransform.anchoredPosition = Vector2.zero;
        gameObject.SetActive(true);
    }


    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }


    public virtual void Close()
    {
        UIManager.CloseUIById(uid);
    }

    protected virtual void OnDestroy()
    {

    }
}
