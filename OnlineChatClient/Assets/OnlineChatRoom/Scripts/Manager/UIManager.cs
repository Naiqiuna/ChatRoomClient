using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    private static Dictionary<string, UIPanelBase> uiPanelPool = new Dictionary<string, UIPanelBase>();
    private static Dictionary<int, UIPanelBase> openingPanels = new Dictionary<int, UIPanelBase>();
    private static List<int> idPools = new List<int>();
    private static Transform window;
    private static Transform main;
    private static GameObject poolObj;
    private int uid = 0;
    private const int maxIdCount = 100;

    private Canvas ui;
  

    private void Awake()
    {
        ui = GameObject.Find("Canvas").GetComponent<Canvas>();
        poolObj = new GameObject("UIPool");
        main = ui.transform.Find("Main");
        window = ui.transform.Find("Window");
        idPools = new List<int>();
        for (int i = 0; i < maxIdCount; i++)
        {
            idPools.Add(i);
        }
    }


    private static int GetRandomId()
    {
        int m_id = -1;
        if (idPools.Count > 1)
        {
            do
            {
                m_id = Random.Range(0, maxIdCount);
            } while (!idPools.Contains(m_id));
        }
        return m_id;
    }



    public static UIPanelBase CreateUIByPath(string path)
    {
        UIPanelBase panel;
        if (!uiPanelPool.TryGetValue(path,out panel))
        {
            int m_id = GetRandomId();
            if (m_id == -1) return null;
            panel = Instantiate(Resources.Load<GameObject>(path)).GetComponent<UIPanelBase>();
            panel.UID = m_id;
        }
        switch (panel.Type)
        {
            case PanelType.None:
                break;
            case PanelType.Main:
                panel.transform.parent = main;
                break;
            case PanelType.Window:
                panel.transform.parent = window;
                break;
            default:
                break;
        }
        if (!openingPanels.ContainsKey(panel.UID)) openingPanels.Add(panel.UID, panel);
        panel.Enter();
        return panel;
    }

    public static void CreateWindowUI(string path)
    {

    }


    public static void CloseUIById(int id)
    {
        UIPanelBase panel;
        if (openingPanels.TryGetValue(id,out panel))
        {
            panel.Exit();
            panel.transform.parent = poolObj.transform;
            openingPanels.Remove(id);
        }
        else
        {
            Debug.Log("openingPanels not contains panel what's id = " + id);
        }
    }

    


}
