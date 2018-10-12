using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogoPanel : UIPanelBase {

    [SerializeField] Text enText;
    [SerializeField] Text cnText;

    
    protected override void Start()
    {
        base.Start();
        enText.color = new Color(enText.color.r, enText.color.g, enText.color.b, 0);
        cnText.color = new Color(cnText.color.r, cnText.color.g, cnText.color.b, 0);
        enText.DOFade(1, 2.0f).OnComplete(() => { cnText.DOFade(1, 2.0f).OnComplete(() => {  LoadLoginPanel(); }); });
    }

    private void LoadLoginPanel()
    {
        UIManager.CreateUIByPath(PathManager.LoginPanelPath);
        Close();
    }
}
