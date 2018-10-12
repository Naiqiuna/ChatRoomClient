using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour {

    protected ActionCode actionCode = ActionCode.None;
    protected RequestCode requestCode = RequestCode.None;
    private GameFacade mGameFacade;
    protected GameFacade gameFacade
    {
        get
        {
            if (mGameFacade == null)
                mGameFacade = GameFacade.Instance;
            return mGameFacade;
        }
    }
    protected virtual void Awake() { }

    protected virtual void Start()
    {
        gameFacade.AddRequest(actionCode, this);
    }

    public virtual void SendRequest(string data)
    {
        gameFacade.SendRequest(requestCode, actionCode, data);
    }

    public virtual void OnResponse(ResponseCode code,string data)
    {
        
    }

    public virtual void OnDestroy()
    {
        if (gameFacade != null)
            gameFacade.RemoveRequest(actionCode);
    }

}
