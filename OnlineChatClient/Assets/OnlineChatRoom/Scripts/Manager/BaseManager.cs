﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager{

    protected GameFacade facade;
	public BaseManager(GameFacade facade)
    {
        this.facade = facade;
    }
}
